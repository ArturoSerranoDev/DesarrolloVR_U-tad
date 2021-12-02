using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationSwitcher : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleTeleportAction;
    [SerializeField] private XRRayInteractor teleportRayInteractor;
    
    private void OnEnable()
    {
        toggleTeleportAction.action.started += ToggleRay;
    }
    
    private void OnDisable()
    {
        toggleTeleportAction.action.started -= ToggleRay;
    }

    private void ToggleRay(InputAction.CallbackContext obj)
    {
        teleportRayInteractor.enabled = !teleportRayInteractor.enabled;
    }
}
