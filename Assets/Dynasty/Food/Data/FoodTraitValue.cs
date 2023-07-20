﻿using System;
using Dynasty.Food.Instance;
using UnityEngine;

namespace Dynasty.Food.Data {

[Serializable]
public struct FoodTraitValue {
    [SerializeField] FoodTraitSelection _selection;
    [SerializeField] int _intValue;
    [SerializeField] float _floatValue;
    [SerializeField] bool _boolValue;

    public int Hash => _selection.Hash;
    public FoodTraitType Type => _selection.Type;
    
    public object GetValue() {
        return Type switch {
            FoodTraitType.Int => _intValue,
            FoodTraitType.Float => _floatValue,
            FoodTraitType.Bool => _boolValue,
            FoodTraitType.Tag => null,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void Apply(FoodBehaviour behaviour) {
        behaviour.SetTrait(Hash, Type, GetValue());
    }
}

}