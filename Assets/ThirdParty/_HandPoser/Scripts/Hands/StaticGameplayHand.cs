// ----------------------------------------------------------------------------
// StaticGameplayHand.cs
//
// Author: Arturo Serrano
// Date: 31/01/21
// Copyright: © Arturo Serrano
//
// Brief: Hand poser derived from GameplayHand that leaves the pose at grabbed object
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StaticGameplayHand : BaseHand
{
    // The interactor we react to
    [SerializeField] private XRBaseInteractor targetInteractor = null;

    private void OnEnable()
    {
        // Subscribe to selected events
        targetInteractor.onSelectEntered.AddListener(TryApplyObjectPose);
        targetInteractor.onSelectExited.AddListener(TryApplyDefaultPose);
    }

    private void OnDisable()
    {
        // Unsubscribe to selected events
        targetInteractor.onSelectEntered.RemoveListener(TryApplyObjectPose);
        targetInteractor.onSelectExited.RemoveListener(TryApplyDefaultPose);
    }

    private void TryApplyObjectPose(XRBaseInteractable interactable)
    {
        // Try and get pose container, and apply
        if (interactable.TryGetComponent(out PoseContainer poseContainer))
        {
            // Set parent at interactor
            transform.parent = interactable.transform;

            ApplyPose(poseContainer.pose);


        }
    }

    private void TryApplyDefaultPose(XRBaseInteractable interactable)
    {
        // Try and get pose container, and apply
        if (interactable.TryGetComponent(out PoseContainer poseContainer))
        {
            // Set parent at interactable
            transform.parent = targetInteractor.transform;

            ApplyDefaultPose();

        }
    }

    public override void ApplyOffset(Vector3 position, Quaternion rotation)
    {
        // Invert since the we're moving the attach point instead of the hand
        Vector3 finalPosition = position * -1.0f;
        Quaternion finalRotation = Quaternion.Inverse(rotation);

        // Since it's a local position, we can just rotate around zero
        finalPosition = finalPosition.RotatePointAroundPivot(Vector3.zero, finalRotation.eulerAngles);

        // Set the position and rotach of attach
        transform.localPosition = position;
        transform.localRotation = rotation;

    }

    private void OnValidate()
    {
        // Let's have this done automatically, but not hide the requirement
        if (!targetInteractor)
            targetInteractor = GetComponentInParent<XRBaseInteractor>();
    }
}
