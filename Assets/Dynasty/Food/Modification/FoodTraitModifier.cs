﻿using System;
using Dynasty.Food.Data;
using Dynasty.Food.Instance;
using Dynasty.Library.Classes;
using UnityEngine;

namespace Dynasty.Food.Modification {

[Serializable]
public class FoodTraitModifier {
    [SerializeField] FoodTraitSelection _trait;
    [SerializeField] Operation _operation;

    [SerializeField] Modifier _modifier;
    [SerializeField] int _intValue;
    [SerializeField] float _floatValue;
    [SerializeField] bool _boolValue;
    
    public void Apply(FoodBehaviour food) {
        _trait.GetEntry(out var trait);
        if (trait.Type == FoodTraitType.Tag) {
            food.AddTag(trait.Hash);
            return;
        }

        switch (_operation) {
            case Operation.Set:
                SetTrait(food, trait); break;
            case Operation.Modify:
                ModifyTrait(food, trait); break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    void SetTrait(FoodBehaviour food, FoodTraitDatabase.Entry trait) {
        switch (trait.Type) {
            case FoodTraitType.Int:
                food.SetTrait(trait.Hash, _intValue); break;
            case FoodTraitType.Float:
                food.SetTrait(trait.Hash, _floatValue); break;
            case FoodTraitType.Bool:
                food.SetTrait(trait.Hash, _boolValue); break;
            case FoodTraitType.Tag:
            default: throw new ArgumentOutOfRangeException();
        }
    }

    void ModifyTrait(FoodBehaviour food, FoodTraitDatabase.Entry trait) {
        switch (trait.Type) {
            case FoodTraitType.Float:
                var value = food.GetTrait<float>(trait.Hash); 
                food.SetTrait(trait.Hash, _modifier.Apply(value));
                break;
            case FoodTraitType.Int:
                var intValue = food.GetTrait<int>(trait.Hash);
                food.SetTrait(trait.Hash, _modifier.Apply(intValue));
                break;
            case FoodTraitType.Bool:
                throw new InvalidOperationException("Cannot modify bool traits");
            case FoodTraitType.Tag:
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public enum Operation {
        Set,
        Modify
    }
}

}