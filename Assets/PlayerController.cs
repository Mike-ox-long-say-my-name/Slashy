using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField, Min(0)] private float horizontalMoveSpeed = 5;
    [SerializeField, Min(0)] private float verticalMoveSpeed = 2;

    [SerializeField, Min(0)] private float jumpStartVelocity = 5;

    [SerializeField] private Camera followingCamera;
    [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

    [SerializeField] private CharacterController characterController;

    private Vector3 _velocity;
    private float _cameraFollowVelocity;

    private void Awake()
    {
        if (followingCamera == null)
        {
            followingCamera = Camera.main;
        }
    }

    private void Update()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        var move = new Vector3(hor * horizontalMoveSpeed, 0, ver * verticalMoveSpeed);
        _velocity += move;

        if (characterController.isGrounded)
        {
            _velocity.y = 0;
            if (Input.GetButtonDown("Jump"))
            {
                _velocity.y = jumpStartVelocity;
            }
        }
        _velocity.y += gravity * Time.deltaTime;
        
        characterController.Move(_velocity * Time.deltaTime);

        _velocity.x = 0;
        _velocity.z = 0;
        
        var cameraPosition = followingCamera.transform.position;
        var newX = Mathf.SmoothDamp(cameraPosition.x, transform.position.x,
            ref _cameraFollowVelocity, followSmoothTime);
        followingCamera.transform.position = new Vector3(newX, cameraPosition.y, cameraPosition.z);
    }
}
