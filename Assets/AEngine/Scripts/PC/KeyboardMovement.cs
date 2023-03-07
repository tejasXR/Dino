using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movementSpeed = 4F;
    [SerializeField] private float gravity = 9.18F;

    private Vector3 _velocity;
    private void Update()
    {
        MoveCharacter();
        EnforceGravity();
    }

    private void MoveCharacter()
    {
        var xMovement = Input.GetAxis("Horizontal");
        var zMovement = Input.GetAxis("Vertical");

        var move = transform.right * xMovement + transform.forward * zMovement;
        characterController.Move(move * (movementSpeed * Time.deltaTime));
    }

    private void EnforceGravity()
    {
        _velocity.y += -gravity * Time.deltaTime;
        characterController.Move(_velocity * Time.deltaTime);

        if (characterController.isGrounded)
            _velocity = Vector3.zero;
    }
}
