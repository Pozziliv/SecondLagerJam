using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    [SerializeField] private Transform _camera;

    private Vector3 _direction;
    private float _horizontal, _vertical;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        _direction = new Vector3(_horizontal, 0,_vertical).normalized;
        _direction = _camera.TransformDirection(_direction);
        _direction = new Vector3(_direction.x, 0, _direction.z).normalized * _speed;
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(_direction.x, _rb.velocity.y, _direction.z);
    }
}
