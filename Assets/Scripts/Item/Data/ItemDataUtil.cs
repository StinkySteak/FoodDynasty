﻿using System;
using System.Collections.Generic;

public static class ItemDataUtil {
    static readonly Dictionary<ItemDataType, Type> _itemDataLookup = new() {
        { ItemDataType.Cookable, typeof(Cookable) },
        { ItemDataType.Spice, typeof(Spice) },
        { ItemDataType.Purified, typeof(Purified) }
    };
    
    public static Type GetDataType(ItemDataType type) {
        return _itemDataLookup[type];
    }
}

public enum ItemDataType {
    Cookable,
    Spice,
    Purified
}