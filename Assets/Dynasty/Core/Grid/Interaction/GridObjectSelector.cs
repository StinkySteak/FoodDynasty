﻿using Dynasty.Core.Tooltip;
using Dynasty.Library.Events;
using Dynasty.Library.Extensions;
using UnityEngine;

namespace Dynasty.Core.Grid {

public class GridObjectSelector : MonoBehaviour {
    [SerializeField] GameEvent<GridObject> _selectObjectEvent;
    [SerializeField] GameEvent<GridObject>[] _deselectObjectEvents;
    [SerializeField] TooltipData<Library.Entity.Entity> _tooltipData;

    GridObject _selectedObject;
    Outline _selectedOutline;
    
    void OnEnable() {
        _selectObjectEvent.AddListener(OnObjectSelected);
        foreach (var deselectEvent in _deselectObjectEvents) {
            deselectEvent.AddListener(OnObjectDeselected);
        }
    }
    
    void OnDisable() {
        _selectObjectEvent.RemoveListener(OnObjectSelected);
        foreach (var deselectEvent in _deselectObjectEvents) {
            deselectEvent.RemoveListener(OnObjectDeselected);
        }
    }

    void OnObjectSelected(GridObject obj) {
        ChangeSelection(obj);
    }
    
    void OnObjectDeselected(GridObject obj) {
        if (_selectedObject != obj) return;
        ChangeSelection(null);
    }

    void ChangeSelection(GridObject newSelection) {
        if (_selectedObject != null) {
            _selectedOutline.enabled = false;
            _selectedOutline = null;
            _tooltipData.Hide();
        }

        _selectedObject = newSelection; 
        if (_selectedObject == null) return;

        _selectedOutline = _selectedObject.GetOrAddComponent<Outline>();
        _selectedOutline.enabled = true;
        
        if (!_selectedObject.TryGetComponent(out Library.Entity.Entity entity)) return;
        _tooltipData.Show(entity);
    }
}

}