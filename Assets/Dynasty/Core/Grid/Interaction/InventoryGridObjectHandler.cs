﻿using Dynasty.Library;
using Dynasty;
using UnityEngine;

namespace Dynasty.Grid {

/// <summary>
/// Handles interaction between <see cref="InventoryAsset"/> and <see cref="GridObject"/>s.
/// </summary>
public class InventoryGridObjectHandler : MonoBehaviour {
    [SerializeField] GridObjectBuilder _builder;
    [SerializeField] InventoryAsset _inventory;
    
    [Space]
    
    [Tooltip("When raised, adds the given object back to the inventory.")]
    [SerializeField] GameEvent<GridObject> _deleteObjectEvent;
    
    [Tooltip("When raised, attempts to start building the given item.")]
    [SerializeField] GameEvent<Item> _startBuildingEvent;

    void OnEnable() {
        _deleteObjectEvent.AddListener(OnGridObjectDeleted);
        _startBuildingEvent.AddListener(OnStartBuilding);
    }
    
    void OnDisable() {
        _deleteObjectEvent.RemoveListener(OnGridObjectDeleted);
        _startBuildingEvent.RemoveListener(OnStartBuilding);
    }
    
    void OnGridObjectDeleted(GridObject gridObject) {
        RegisterDeletion(gridObject);
    }

    public bool RegisterDeletion(GridObject gridObject) {
        if (!gridObject.TryGetComponent(out MachineEntity machineEntity)) return false;
        
        foreach (var handler in gridObject.GetComponentsInChildren<IOnDeletedHandler>()) {
            handler.OnDeleted(_inventory);
        }
        _inventory.Add(machineEntity.Data);
        return true;
    }

    async void OnStartBuilding(Item item) {
        if (item.Inventory != _inventory) return;
        if (item.Data is not IPrefabProvider<GridObject> prefabProvider) return;
        
        await _builder.StartPlacing(prefabProvider.Prefab, BeforePlace, AfterPlace);
        return;

        void AfterPlace(GridObject obj, GridPlacementResult result) {
            if (result.WasSuccessful) return;
            _inventory.Add(item.Data);
        }
        
        bool BeforePlace() {
            return _inventory.Remove(item.Data);
        }
    }
}

}