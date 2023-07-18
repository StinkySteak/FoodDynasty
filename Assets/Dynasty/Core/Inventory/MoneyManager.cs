﻿using System;
using Dynasty.Library.Events;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dynasty.Core.Inventory {

/// <summary>
/// Manages the player's money.
/// </summary>
[CreateAssetMenu(menuName = "Manager/Money")]
public class MoneyManager : ScriptableObject {
    [Tooltip("Raised when the player's money changes.")] 
    [FormerlySerializedAs("_moneyChangedEvent")] 
    [SerializeField] ValueChangedEvent<double> _onMoneyChanged;

    double _currentMoney;

    /// <summary>
    /// The player's current money. Cannot be negative.
    /// </summary>
    public double CurrentMoney {
        get => _currentMoney;
        set {
            var clamped = Math.Max(value, 0);
            if (Math.Abs(clamped - _currentMoney) < 0.05f) return;
            var previous = _currentMoney;
            
            _currentMoney = clamped;
            _onMoneyChanged.Raise(previous, _currentMoney);
            OnMoneyChanged?.Invoke(previous, _currentMoney);
        }
    }
    
    /// <summary>
    /// Raised when the player's money changes.
    /// </summary>
    public event Action<double, double> OnMoneyChanged;
}

}