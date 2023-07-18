﻿using UnityEngine;

namespace Dynasty.Core.Data {

/// <summary>
/// Provides a prefab, see <see cref="IDataProvider{T}"/>.
/// </summary>
public interface IPrefabProvider<out T> : IDataProvider<T> where T : Component { 
    T Prefab { get; }
    T IDataProvider<T>.Data => Prefab;
}

}