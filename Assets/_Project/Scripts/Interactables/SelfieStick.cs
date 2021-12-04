using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class SelfieStick : MonoBehaviour
{
    [SerializeField] private InputActionReference leftJoystickAction;
    [SerializeField] private InputActionReference rightJoystickAction;
    
    [SerializeField] private GameObject extendableStick;
    [SerializeField] private GameObject socketArea;

    [Header("Reticle")]
    [SerializeField] private GameObject feedbackReticle;
    [SerializeField] private float maxReticleOffset = 0.018f;

    private XRGrabInteractable grabInteractable;
    
    private enum Directions { North, East, South, West, None}

    private Directions currentDirection = Directions.None;
    private HandSideType selectingHandSide = HandSideType.None;
    
    private bool isExtended;
    private bool isAnimating;
    
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        leftJoystickAction.action.performed += OnJoystickInputLeft;
        rightJoystickAction.action.performed += OnJoystickInputRight;
        
        grabInteractable.selectEntered.AddListener(GetSelectingHand);
    }
    
    private void OnDisable()
    {
        leftJoystickAction.action.performed -= OnJoystickInputLeft;
        rightJoystickAction.action.performed -= OnJoystickInputRight;
        
        grabInteractable.selectEntered.RemoveListener(GetSelectingHand);
    }

    private void Update()
    {
        if (isAnimating)
            return;
        
        currentDirection = DirectionBasedOnInput();

        switch (currentDirection)
        {
            case Directions.North:
                StartCoroutine(ExtendStick());
                break;
            case Directions.South:
                StartCoroutine(RetractStick());
                break;
            case Directions.East:
                StartCoroutine(RotateStickSocket(180f));
                break;
            case Directions.West:
                StartCoroutine(RotateStickSocket(-180f));
                break;
        }
    }

    private IEnumerator RotateStickSocket(float rotateAmount)
    {
        isAnimating = true;
        socketArea.transform.DOLocalRotate(socketArea.transform.localEulerAngles + new Vector3(0, 0f, rotateAmount), 1);
        yield return new WaitForSeconds(1f);
        isAnimating = false;
    }

    private IEnumerator ExtendStick()
    {
        if (isExtended)
        {
            yield break; // Similar to return in coroutine
        }
        
        isAnimating = true;

        extendableStick.transform.DOScale(Vector3.one, 1f);
        yield return new WaitForSeconds(1f);

        isAnimating = false;
        isExtended = true;
    }
    
    private IEnumerator RetractStick()
    {
        if (!isExtended)
        {
            yield break; // Similar to return in coroutine
        }
        
        isAnimating = true;

        extendableStick.transform.DOScale(new Vector3(1, 0.1f, 1), 1f);
        yield return new WaitForSeconds(1f);
        
        isAnimating = false;
        isExtended = false;
    }

    private Directions DirectionBasedOnInput()
    {
        if (feedbackReticle.transform.localPosition.z > 0.01f)
        {
            return Directions.North;
        }
        else if (feedbackReticle.transform.localPosition.z < -0.01f)
        {
            return Directions.South;
        }
        else if (feedbackReticle.transform.localPosition.x > 0.01f)
        {
            return Directions.East;
        }
        else if (feedbackReticle.transform.localPosition.x < -0.01f)
        {
            return Directions.West;
        }

        return Directions.None;
    }

    private void GetSelectingHand(SelectEnterEventArgs arg0)
    {
        selectingHandSide = arg0.interactorObject.transform.GetComponent<HandSide>().handSide;
    }

    #region Input

    private void OnJoystickInputLeft(InputAction.CallbackContext obj)
    {
        if (selectingHandSide != HandSideType.Left)
            return;
        
        Vector2 joystickInputValue = obj.ReadValue<Vector2>();
        
        feedbackReticle.transform.localPosition = new Vector3(joystickInputValue.x * maxReticleOffset, 
                                                                0, 
                                                                joystickInputValue.y * maxReticleOffset);
    }
    
    private void OnJoystickInputRight(InputAction.CallbackContext obj)
    {
        if (selectingHandSide != HandSideType.Right)
            return;
        
        Vector2 joystickInputValue = obj.ReadValue<Vector2>();
        
        feedbackReticle.transform.localPosition = new Vector3(joystickInputValue.x * maxReticleOffset, 
                                                                0, 
                                                                joystickInputValue.y * maxReticleOffset);
    }

    #endregion
   
}
