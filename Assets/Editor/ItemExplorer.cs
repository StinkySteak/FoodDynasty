using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

public class ItemExplorer : EditorWindow {
    [SerializeField] VisualTreeAsset _mainUxml;
    [SerializeField] VisualTreeAsset _listItemUxml;
    [SerializeField] VisualTreeAsset _machineEditorUxml;
    [SerializeField] VisualTreeAsset _componentEditorUxml;
    
    ItemData _selectedItem;
    List<ItemData> _items;
    ItemMachineEditor _machineEditor;

    [MenuItem("Window/Item Explorer")]
    public static void ShowWindow() {
        var window = GetWindow<ItemExplorer>();
        
        window.titleContent = new GUIContent("Item Explorer");
        window.minSize = new Vector2(500, 300);
    }

    public void CreateGUI() {
        var root = rootVisualElement;
        _mainUxml.CloneTree(root);

        root.Q<Button>(name: "generate-image-button").clicked += () => {
            if (_selectedItem == null || _selectedItem is not IPrefabProvider<GridObject> prefabProvider) return;
            _selectedItem.Image = ThumbnailCreator.Create(prefabProvider.Prefab, _selectedItem.Name);
        };

        var createMenu = root.Q<ToolbarMenu>("item-create-menu");
        ConfigureCreateMenu(createMenu.menu, createMenu.worldBound);

        root.Q<ToolbarButton>("refresh-items").clicked += RefreshItems;

        RefreshItems();
        ConfigureItemList(root.Q<ListView>("itemlist"));
    }

    void RefreshItems() {
        _items = EditorUtil.FetchAll<ItemData>().ToList();
    }

    void ConfigureCreateMenu(DropdownMenu menu, Rect pos) {
        foreach (var createOption in EnumHelpers.GetValues<ItemCreatorPopupWindow.CreateMode>()) {
            var displayName = EditorUtil.FormatDisplay(createOption.ToString().Replace("_", "/"));
            
            menu.AppendAction(displayName, _ => {
                var popup = new ItemCreatorPopupWindow(createOption);
                popup.OnItemCreated += RefreshItems;
            
                PopupWindow.Show(pos, popup);
            });
        }
    }
    
    void ConfigureItemList(BaseVerticalCollectionView listView) {
        listView.makeItem = MakeItem;
        listView.bindItem = BindItem;
        listView.itemsSource = _items;

        listView.onSelectionChange += objects => {
            SelectItem(objects.FirstOrDefault() as ItemData);
        };
        
        listView.SetSelection(0);
        
        VisualElement MakeItem() {
            return _listItemUxml.CloneTree();
        }

        void BindItem(VisualElement e, int i) {
            e.Q<Label>().text = _items[i].Name;
            e.Q(name: "item-icon").style.backgroundImage = _items[i].Image.texture;
        }
    }

    void SelectItem(ItemData itemData) {
        if (itemData == null) return;
        _selectedItem = itemData;

        var generateImageButton = rootVisualElement.Q<Button>(name: "generate-image-button");
        generateImageButton.SetEnabled(itemData is IPrefabProvider<GridObject>);
        
        var generalItemEditor = rootVisualElement.Q("editor-general");
        
        generalItemEditor.Unbind();
        generalItemEditor.Bind(new SerializedObject(_selectedItem));

        if (itemData is MachineItemData machineItemData) {
            if (_machineEditor == null) {
                var editorRoot = _machineEditorUxml.CloneTree();
                _machineEditor = new ItemMachineEditor(editorRoot, _componentEditorUxml);
                generalItemEditor.Add(editorRoot);
            }
            
            _machineEditor.Show(machineItemData);
        } else {
            _machineEditor?.Hide();
        }
    }
}