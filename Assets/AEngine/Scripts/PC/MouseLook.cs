using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private Camera playerCamera;
    [Space(5)]
    [SerializeField] private float mouseSensitivity = 100F;
    [SerializeField] private float horizontalFOV = 90F;

    private float _xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -horizontalFOV, horizontalFOV);
        
        playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0F, 0F);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
