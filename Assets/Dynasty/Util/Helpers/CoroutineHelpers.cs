﻿using System.Collections.Generic;
using UnityEngine;

public static class CoroutineHelpers {
    static readonly Dictionary<float, WaitForSeconds> _waitForSeconds = new();
    
    public static WaitForSeconds Wait(float seconds) {
        if (_waitForSeconds.TryGetValue(seconds, out var waitForSeconds)) {
            return waitForSeconds;
        }
        waitForSeconds = new WaitForSeconds(seconds);
        _waitForSeconds.Add(seconds, waitForSeconds);
        return waitForSeconds;
    }
}