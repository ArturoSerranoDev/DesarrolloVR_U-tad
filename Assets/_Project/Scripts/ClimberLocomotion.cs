using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimberLocomotion : MonoBehaviour
{
    [Header("Input Position")]
    [SerializeField] private InputActionReference leftControllerPosAction;
    [SerializeField] private InputActionReference rightControllerPosAction;

    [SerializeField] private ContinuousMoveProviderBase playerMoveProvider;
    [SerializeField] private CharacterController characterController;
    
    private List<HandSideType> selectingHands;
    private List<XRSimpleInteractable> climbableInteractables;

    private Vector3 lastLeftHandPos;
    private Vector3 lastRightHandPos;

    private bool isClimbLocomotionEnabled;

    // Start is called before the first frame update
    void Awake()
    {
        climbableInteractables = new List<XRSimpleInteractable>();
        selectingHands = new List<HandSideType>();

        GameObject[] objectList = GameObject.FindGameObjectsWithTag("ClimbInteractable");

        for (int i = 0; i < objectList.Length; i++)
        {
            climbableInteractables.Add(objectList[i].GetComponent<XRSimpleInteractable>());
        }

    }

    private void OnEnable()
    {
        foreach (var climberInteractable in climbableInteractables)
        {
            climberInteractable.selectEntered.AddListener(OnClimbInteractableSelected);
            climberInteractable.selectExited.AddListener(OnClimbInteractableExited);
        }
    }

    private void OnDisable()
    {
        foreach (var climberInteractable in climbableInteractables)
        {
            climberInteractable.selectEntered.RemoveListener(OnClimbInteractableSelected);
            climberInteractable.selectExited.RemoveListener(OnClimbInteractableExited);
        }
    }

    private void Update()
    {
        if (!isClimbLocomotionEnabled)
            return;

        CalculateClimbingMovement();
    }

    private void CalculateClimbingMovement()
    {
        Vector3 movement = Vector3.zero;

        if (selectingHands.Contains(HandSideType.Left))
        {
            // Get delta movement of left hand
            Vector3 controllerPos = leftControllerPosAction.action.ReadValue<Vector3>();
            Vector3 velocity = controllerPos - lastLeftHandPos;

            if (velocity != Vector3.zero)
                movement -= velocity;

            lastLeftHandPos = controllerPos;
        }
        
        if (selectingHands.Contains(HandSideType.Right))
        {
            // Get delta movement of right hand
            Vector3 controllerPos = rightControllerPosAction.action.ReadValue<Vector3>();
            Vector3 velocity = controllerPos - lastRightHandPos;

            if (velocity != Vector3.zero)
                movement -= velocity;

            lastRightHandPos = controllerPos;
        }

        characterController.Move(movement);
    }

    private void OnClimbInteractableSelected(SelectEnterEventArgs selectEnterEventData)
    {
        // Store hand that selected climbable
        HandSideType selectedHand = selectEnterEventData.interactorObject.transform.
              GetComponentInChildren<HandSide>().handSide;

        selectingHands.Add(selectedHand);


        if (selectedHand == HandSideType.Left)
        {
            lastLeftHandPos = leftControllerPosAction.action.ReadValue<Vector3>();
        }
        else
        {
            lastRightHandPos = rightControllerPosAction.action.ReadValue<Vector3>();
        }

        // If there is at least 1 hand grabbing a climbable, activate this locomotion
        // and disable regular locomotion
        if (selectingHands.Count > 0)
        {
            isClimbLocomotionEnabled = true;
            playerMoveProvider.enabled = false;
        }
    }
    
    private void OnClimbInteractableExited(SelectExitEventArgs selectExitedEventData)
    {
        // Discard selecting hand
        HandSideType unselectedHand = selectExitedEventData.interactorObject.transform.
                      GetComponentInChildren<HandSide>().handSide;

        selectingHands.Remove(unselectedHand);

        if(unselectedHand == HandSideType.Left)
        {
            lastLeftHandPos = Vector3.zero;
        }
        else
        {
            lastRightHandPos = Vector3.zero;
        }

        // If there are 0 hands grabbing climbable interactables, disable this locomotion
        // and activate regular locomotion
        if (selectingHands.Count <= 0)
        {
            isClimbLocomotionEnabled = false;
            playerMoveProvider.enabled = true;
        }
    }
}
