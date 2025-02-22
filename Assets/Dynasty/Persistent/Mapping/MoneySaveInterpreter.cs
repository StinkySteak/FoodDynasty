﻿using System;
using Dynasty;
using Dynasty.Persistent;
using UnityEngine;

namespace Dynasty.Persistent {

[CreateAssetMenu(menuName = "Saving/Interpreter/Money")]
public class MoneySaveInterpreter : SaveInterpreter<MoneySaveInterpreter.SaveData> {
    [SerializeField] double _startingMoney;
    [SerializeField] MoneyManager _moneyManager;

    protected override SaveData DefaultData => new() {
        Money = _startingMoney,
        TotalMoneyMade = 0
    };

    protected override void OnLoad(SaveData saveData) {
        _moneyManager.CurrentMoney = saveData.Money;
        _moneyManager.TotalMoneyMade = saveData.TotalMoneyMade;
    }

    protected override SaveData GetSaveData() {
        return new SaveData {
            Money = _moneyManager.CurrentMoney,
            TotalMoneyMade = _moneyManager.TotalMoneyMade
        };
    }
    
    [Serializable]
    public struct SaveData {
        public double Money;
        public double TotalMoneyMade;

        public override string ToString() {
            return $"Money: {Money}, TotalMoneyMade: {TotalMoneyMade}";
        }
    }
}

}