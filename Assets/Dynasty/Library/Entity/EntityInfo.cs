﻿using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Dynasty.Library {

public readonly struct EntityInfo {
    public readonly string Name;
    public readonly string Value;
    public readonly Sprite Icon;
    
    public EntityInfo(string name, string value, Sprite icon = null) {
        Name = name;
        Value = value;
        Icon = icon;
    }
    
    bool Equals(EntityInfo other) {
        return Name == other.Name && Value == other.Value && Equals(Icon, other.Icon);
    }

    public override bool Equals(object obj) {
        return obj is EntityInfo other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Name, Value, Icon);
    }
}

}