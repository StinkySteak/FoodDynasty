﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dynasty.Food.Instance;
using UnityEngine;

namespace Dynasty.Food.Filtering {

[CreateAssetMenu(menuName = "Food/Filter")]
public class FoodFilterGroup : ScriptableObject {
    [SerializeField] AndOr _condition;
    [SerializeField] FoodFilter[] _filters;

    public bool Check(FoodBehaviour food) {
        return Check(_filters, food);
    }

    bool Check(IReadOnlyCollection<FoodFilter> filters, FoodBehaviour food) {
        if (filters.Count == 0) return true;
        
        return _condition switch {
            AndOr.All => filters.All(condition => condition.Check(food)),
            AndOr.Any => filters.Any(condition => condition.Check(food)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    enum AndOr {
        All,
        Any
    }
}

}