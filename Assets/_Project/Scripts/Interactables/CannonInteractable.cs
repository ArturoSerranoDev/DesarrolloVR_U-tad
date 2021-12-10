using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CannonInteractable : MonoBehaviour
{
    // VARIABLES
    [Header("Interactables")]
    [SerializeField] private DialInteractable handleRotateCannon;
    [SerializeField] private DialInteractable handleRotateWheel;

    [SerializeField] private XRSimpleInteractable shootButtonInteractable;
    [SerializeField] private XRSocketInteractor dropObjectsArea;

    [Header("Objects")]
    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject wheels;
    [SerializeField] private GameObject cannonStructure;

    private float lastHandleCannonRotation = 0;
    private float lastHandleWheelsRotation = 0;

    private void OnEnable()
    {
        handleRotateCannon.OnDialChanged.AddListener(RotateCannon);
        handleRotateWheel.OnDialChanged.AddListener(MoveCannon);

        shootButtonInteractable.activated.AddListener(ShootButtonActivated);

        dropObjectsArea.selectEntered.AddListener(DropAreaSelected);
    }

    private void OnDisable()
    {
        handleRotateCannon.OnDialChanged.RemoveListener(RotateCannon);
        handleRotateWheel.OnDialChanged.RemoveListener(MoveCannon);

        shootButtonInteractable.activated.RemoveListener(ShootButtonActivated);

        dropObjectsArea.selectEntered.RemoveListener(DropAreaSelected);

    }

    private void DropAreaSelected(SelectEnterEventArgs arg0)
    {
        throw new NotImplementedException();
    }

    private void ShootButtonActivated(ActivateEventArgs arg0)
    {
        throw new NotImplementedException();
    }

    private void MoveCannon(DialInteractable arg0)
    {
        float wheelsDelta = arg0.CurrentAngle - lastHandleWheelsRotation;

        if (wheelsDelta > 30 || wheelsDelta < -30)
            wheelsDelta = 0;

        Debug.Log("Wheels Delta" + wheelsDelta);

        transform.Translate(transform.right * wheelsDelta * Time.deltaTime, Space.World);

        lastHandleWheelsRotation = arg0.CurrentAngle;
    }
    private void RotateCannon(DialInteractable arg0)
    {
        float rotationDelta = arg0.CurrentAngle - lastHandleCannonRotation;

        if (rotationDelta > 30 || rotationDelta < -30)
            rotationDelta = 0;

        Debug.Log("Rotation Delta" + rotationDelta);

        cannon.transform.Rotate(cannon.transform.up, rotationDelta * Time.deltaTime * 5, Space.World);

        float clampedRotation = Mathf.Clamp(cannon.transform.localEulerAngles.y, 5, 60);

        cannon.transform.localEulerAngles = new Vector3(cannon.transform.localEulerAngles.x,
                                                   clampedRotation,
                                                   cannon.transform.localEulerAngles.z);

        lastHandleCannonRotation = arg0.CurrentAngle;
    }

    // THINGS TO DO

    // Able to rotate cannon if rotate the central handle
    // Clamp or limit the value the cannnon can be rotated

    // Able to rotate all cannon when grabbing end handle

    // Able to move forward or backwards when moving wheel handle

    // Able to shoot 
    // If no projectile inside, shouldn't shoot

    // Player has to be able to introduce an interactable to shoot (socket)

    // User has to be able to shoot when grabbing an interactable (maybe timer?)
}
