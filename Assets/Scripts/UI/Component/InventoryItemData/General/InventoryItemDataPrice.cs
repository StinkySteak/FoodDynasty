﻿using TMPro;
using UnityEngine;

public class InventoryItemDataPrice : UIComponent<ItemData> {
    [SerializeField] TMP_Text _text;
    
    public override void SetContent(ItemData content) {
        _text.text = StringHelpers.FormatMoney(content.Price);
    }
}