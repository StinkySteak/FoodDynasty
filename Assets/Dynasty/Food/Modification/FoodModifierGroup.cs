﻿using Dynasty.Food.Instance;
using Dynasty.Library.Classes;
using UnityEngine;

namespace Dynasty.Food.Modification {

[CreateAssetMenu(menuName = "Food/Modifier")]
public class FoodModifierGroup : ScriptableObject {
    [SerializeField] Modifier _sellPriceModifier = new(multiplicative: 1);
    [SerializeField] FoodModelModifier[] _modelModifiers;
    [SerializeField] FoodDataModifier[] _dataModifiers;
    
    public Modifier SellPriceModifier {
        get => _sellPriceModifier;
        set => _sellPriceModifier = value;
    }

    public void Apply(FoodBehaviour food) {
        food.SellPrice += _sellPriceModifier;
        
        foreach (var modifier in _modelModifiers) {
            modifier.Apply(food);
        }

        foreach (var modifier in _dataModifiers) {
            modifier.Apply(food);
        }
    }
}

}