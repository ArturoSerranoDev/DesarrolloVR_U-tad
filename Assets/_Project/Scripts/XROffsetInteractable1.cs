using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetInteractable1 : XRGrabInteractable
{
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        MatchAttachPoint(args.interactorObject);
    }

    private void MatchAttachPoint(IXRSelectInteractor interactor)
    {
        bool isDirectInteractor = interactor is XRDirectInteractor;
        attachTransform.position = isDirectInteractor ? interactor.GetAttachTransform(interactor.firstInteractableSelected).position : transform.position;
        attachTransform.rotation = isDirectInteractor ? interactor.GetAttachTransform(interactor.firstInteractableSelected).rotation : transform.rotation;
    }
}
