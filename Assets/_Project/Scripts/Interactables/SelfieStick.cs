using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SelfieStick : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    
    private void OnEnable()
    {
        moveAction.action.performed += ToggleRay;
    }
    
    private void OnDisable()
    {
        moveAction.action.performed -= ToggleRay;
    }

    private void ToggleRay(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.));
    }
}