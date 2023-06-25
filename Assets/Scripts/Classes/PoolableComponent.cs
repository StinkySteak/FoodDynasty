﻿using System;
using UnityEngine;

public class PoolableComponent<T> : MonoBehaviour, IPoolable<PoolableComponent<T>> where T : Component {
    [SerializeField] T _component;
    
    public T Component => _component;
    
    public event Action<PoolableComponent<T>> OnDisposed;

    public void Dispose() {
        OnDisposed?.Invoke(this);
    }
}