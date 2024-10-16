using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingControl : MonoBehaviour
{
    PlayerInput _playerInput;

    Vector2 mousePos;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void Update()
    {
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _playerInput.Editor.MousePosition.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }
}
