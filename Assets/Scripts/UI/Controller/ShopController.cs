﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour {
    [Header("Shop")]
    [SerializeField] Transform _itemParent;
    [SerializeField] Container<InventoryItemData> _itemPrefab;
    [SerializeField] GameEvent<InventoryItem> _onItemPurchased;
    [SerializeField] ItemSortingMode _sortingMode;
    [SerializeField] InventoryItemData[] _itemData;
    [Space]
    [SerializeField] MoneyManager _moneyManager;
    [SerializeField] TooltipData<InventoryItemData> _tooltipData;

    readonly List<(Interactable interactable, Container<InventoryItemData> container)> _items = new();

    void Awake() {
        foreach (var item in _itemData.Sorted(_sortingMode)) {
            var itemContainer = Instantiate(_itemPrefab, _itemParent);
            itemContainer.SetContent(item);
            
            var interactable = itemContainer.GetOrAdd<Interactable>();
            interactable.OnClicked += OnItemClicked;
            interactable.OnHovered += OnItemHovered;

            _items.Add((interactable, itemContainer));
        }
    }

    void OnEnable() {
        _moneyManager.OnMoneyChanged += OnMoneyChanged;
        OnMoneyChanged(0, _moneyManager.CurrentMoney);
    }
    
    void OnDisable() {
        _moneyManager.OnMoneyChanged -= OnMoneyChanged;
    }

    void OnDestroy() {
        for (var i = 0; i < _items.Count; i++) {
            var (interactable, container) = _items[i];
            interactable.OnClicked -= OnItemClicked;
            _items.Remove((interactable, container));
            i--;
        }
    }

    void TryBuy(InventoryItemData item, int count) {
        var cost = item.Price * count;
        if (_moneyManager.CurrentMoney < cost) return;

        _moneyManager.CurrentMoney -= cost;
        _onItemPurchased.Raise(new InventoryItem { Count = count, Data = item });
    }
    
    void OnMoneyChanged(double previous, double current) {
        foreach (var (interactable, container) in _items) {
            interactable.interactable = current >= container.Content.Price;
        }
    }

    void OnItemClicked(Interactable interactable, PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        TryBuy(GetData(interactable), 1);
    }
    
    void OnItemHovered(Interactable interactable, bool hovered, PointerEventData eventData) {
        _tooltipData.Show(GetData(interactable), hovered);
    }

    InventoryItemData GetData(Interactable interactable) {
        return _items.First(item => item.interactable == interactable).container.Content;
    }
}