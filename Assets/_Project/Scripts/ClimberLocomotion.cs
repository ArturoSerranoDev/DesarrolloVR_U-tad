using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimberLocomotion : MonoBehaviour
{
    [Header("InputActions")] 
    [SerializeField] private InputActionReference leftControllerMovementAction;
    [SerializeField] private InputActionReference rightControllerMovementAction;
    
    [SerializeField] private ContinuousMoveProviderBase moveProvider;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private List<HandSideType> selectingHands;
    
    // public event Action<Vector3> climber 
    private List<XRSimpleInteractable> climberInteractables = new List<XRSimpleInteractable>();
    private Vector3 lastLeftPosition;
    private Vector3 lastRightPosition;
    private bool isClimbLocomotionEnabled;

    // Start is called before the first frame update
    void Awake()
    {
        var objs = GameObject.FindGameObjectsWithTag("ClimbInteractable");

        foreach (var climbGO in objs)
        {
            climberInteractables.Add(climbGO.GetComponent<XRSimpleInteractable>());
        }
    }

    private void OnEnable()
    {
        foreach (var climberInteractable in climberInteractables)
        {
            climberInteractable.selectEntered.AddListener(OnClimbInteractableSelected);
            climberInteractable.selectExited.AddListener(OnClimbInteractableExited);
        }
    }

    private void OnDisable()
    {
        foreach (var climberInteractable in climberInteractables)
        {
            climberInteractable.selectEntered.RemoveListener(OnClimbInteractableSelected);
            climberInteractable.selectExited.RemoveListener(OnClimbInteractableExited);
        }
    }

    private void Update()
    {
        if(!isClimbLocomotionEnabled)
            return;

        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 movement = Vector3.zero;

        if (selectingHands.Contains(HandSideType.Left))
        {
            Vector3 controllerPos = leftControllerMovementAction.action.ReadValue<Vector3>();
            Vector3 velocity = lastLeftPosition - controllerPos;
            
            if (lastLeftPosition != Vector3.zero)
                movement += velocity;
            
            lastLeftPosition = controllerPos;
        }
        
        if (selectingHands.Contains(HandSideType.Right))
        {
            Vector3 controllerPos = rightControllerMovementAction.action.ReadValue<Vector3>();
            Vector3 velocity = lastRightPosition - controllerPos;
            
            if (lastRightPosition != Vector3.zero)
                movement += velocity;
            
            lastRightPosition = controllerPos;
        }

        characterController.Move(movement);
    }

    private void OnClimbInteractableSelected(SelectEnterEventArgs arg0)
    {
        // Store hand that selected it
        selectingHands.Add(arg0.interactorObject.transform.GetComponentInChildren<HandSide>().handSide);
        
        // If there is at least 1 hand selecting a climber, enable this locomotion and disable usual locomotion.
        if (selectingHands.Count > 0)
        {
            isClimbLocomotionEnabled = true;
            moveProvider.enabled = false;
        }
    }
    
    private void OnClimbInteractableExited(SelectExitEventArgs arg0)
    {
        // discard selecting hand
        HandSideType handUnselected = arg0.interactorObject.transform.GetComponentInChildren<HandSide>().handSide;
        selectingHands.Remove(handUnselected);

        if (handUnselected == HandSideType.Right)
        {
            lastRightPosition = Vector3.zero;
        }
        else
        {
            lastLeftPosition = Vector3.zero;
        }
        
        // If there are 0 hands selecting, disable this locomotion and enable regular
        if (selectingHands.Count <= 0)
        {
            isClimbLocomotionEnabled = false;
            moveProvider.enabled = true;
        }
    }
}
