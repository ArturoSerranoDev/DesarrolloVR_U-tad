using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NestedInteractor : MonoBehaviour
{
    private XRGrabInteractable parentInteractable;

    private XRDirectInteractor nestedDirectInteractor;
    private ActionBasedController dummyXRController;

    private void Awake()
    {
        parentInteractable = GetComponentInParent<XRGrabInteractable>();
        
        nestedDirectInteractor = GetComponent<XRDirectInteractor>();
        dummyXRController = GetComponent<ActionBasedController>();
    }

    private void OnEnable()
    {
        parentInteractable.activated.AddListener(InjectControllerData);
        parentInteractable.deactivated.AddListener(RemoveControllerData);
    }

    private void OnDisable()
    {
        parentInteractable.activated.RemoveListener(InjectControllerData);
        parentInteractable.deactivated.RemoveListener(RemoveControllerData);
    }
    
    private void RemoveControllerData(DeactivateEventArgs arg0)
    {
        nestedDirectInteractor.xrController = dummyXRController;
    }

    private void InjectControllerData(ActivateEventArgs arg0)
    {
        InjectSelectingXRController(arg0.interactorObject.transform.GetComponent<XRBaseControllerInteractor>());
    }

    private void InjectSelectingXRController(XRBaseControllerInteractor controllerInteractor)
    {
        if (controllerInteractor)
        {
            nestedDirectInteractor.xrController = controllerInteractor.xrController;
        }
    }
}
