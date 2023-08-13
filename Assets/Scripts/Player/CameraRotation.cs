using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private Transform _root;

    private float _rotationY = 0;

    [SerializeField] private float _sensivity = 5f;
    [SerializeField] private float _maxUp = 55f;
    [SerializeField] private float _maxDown = -55f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _transform = transform;
    }

    private void Update()
    {
        _rotationY += Input.GetAxis("Mouse Y") * _sensivity;
        _rotationY = Mathf.Clamp(_rotationY, _maxDown, _maxUp);
        float rotationX = _root.eulerAngles.y + Input.GetAxis("Mouse X") * _sensivity;
        _root.eulerAngles = new Vector3(0, rotationX, 0);
        _transform.localEulerAngles = new Vector3(-_rotationY, 0, 0);
    }
}
