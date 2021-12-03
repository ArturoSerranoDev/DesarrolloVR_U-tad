using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetInteractable : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        MatchAttachTransform(args);
    }

    private void MatchAttachTransform(SelectEnterEventArgs args)
    {
        IXRSelectInteractor interactor = args.interactorObject;

        bool isDirectInteractor = interactor is XRDirectInteractor;

        if (isDirectInteractor)
        {
            attachTransform.position = interactor.GetAttachTransform(interactor.firstInteractableSelected).position;
            attachTransform.rotation = interactor.GetAttachTransform(interactor.firstInteractableSelected).rotation;
        }
        else
        {
            attachTransform.position = transform.position;
            attachTransform.rotation = transform.rotation;
        }
    }
}
