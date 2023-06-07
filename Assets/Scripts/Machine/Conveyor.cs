﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Conveyor : MonoBehaviour {
    [SerializeField] DataObject<float> _speed;

    Rigidbody _rigidbody;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        var newPosition = _rigidbody.position;
        var pos = newPosition;
        
        newPosition += -transform.forward * (_speed.Value * Time.fixedDeltaTime);
        _rigidbody.position = newPosition;
        
        _rigidbody.MovePosition(pos);
    }
}