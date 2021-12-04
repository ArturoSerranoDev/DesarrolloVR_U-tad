using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MultiSocketInteractor : XRBaseInteractor
{
    [SerializeField] private XRInteractionManager interactionManager;
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // interactionManager.CancelInteractorSelection(this);
        
        GameObject attachPoint = Instantiate(new GameObject("attachPoint"), args.interactableObject.transform.position, quaternion.identity);

        args.interactableObject.transform.parent = attachPoint.transform;
        args.interactableObject.transform.GetComponent<Rigidbody>().isKinematic = true;
    }
}
