using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationSwitcher : MonoBehaviour
{
    [SerializeField] private InputActionReference teleportActionToggle;
    private XRRayInteractor teleportInteractor;

    private void Awake()
    {
        teleportInteractor = GetComponent<XRRayInteractor>();
    }
    private void OnEnable()
    {
        teleportActionToggle.action.performed += ToggleRayInteractor;
    }

    private void OnDisable()
    {
        teleportActionToggle.action.performed -= ToggleRayInteractor;
    }

    private void ToggleRayInteractor(InputAction.CallbackContext obj)
    {
        teleportInteractor.enabled = !teleportInteractor.enabled;
    }
}
