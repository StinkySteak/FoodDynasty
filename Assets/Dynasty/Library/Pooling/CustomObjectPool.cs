﻿using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Dynasty.Library {

public abstract class CustomObjectPool<T> : ScriptableObject, IDisposable where T : Component, IPoolable<T> {
    [Header("Object Pool")]
    [SerializeField] T _prefab;
    [SerializeField] bool _collectionCheck = true;
    [SerializeField] int _defaultCapacity = 10;
    [SerializeField] int _maxSize = 10000;
    [SerializeField] bool _clearOnSceneChange = true;
    
    ObjectPool<T> _pool;

    public T Prefab {
        get => _prefab;
        set => _prefab = value;
    }

    protected virtual ObjectPool<T> Pool => _pool ??= new ObjectPool<T>(
        Create, OnGet, OnRelease, Destroy, 
        _collectionCheck, 
        _defaultCapacity, 
        _maxSize
    );

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (_clearOnSceneChange) {  
            Clear();
        }
    }

    protected virtual void OnGet(T obj) {
        obj.SetActive(true);
    }
    
    protected virtual void OnRelease(T obj) {
        obj.SetActive(false);
    }

    protected virtual T Create() {
        var obj = Instantiate(_prefab);
        DontDestroyOnLoad(obj);
        obj.OnDisposed += Release;

        obj.SetActive(false);
        return obj;
    }
    
    protected virtual void Destroy(T obj) {
        if (obj == null) return;
        
        obj.OnDisposed -= Release;
        Destroy(obj.gameObject);
    }

    public virtual void Dispose() {
        _pool?.Dispose();
    }
    
    void Clear() {
        _pool?.Clear();
    }

    protected void Release(T obj) => Pool.Release(obj);
    public T Get() => Pool.Get();
}

}