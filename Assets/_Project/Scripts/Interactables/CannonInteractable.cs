using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CannonInteractable : MonoBehaviour
{
  // VARIABLES
  [SerializeField] private XRInteractionManager interactionManager;
  [SerializeField] private XROrigin player;

  [SerializeField] private DialInteractable handleRotateCannon;
  [SerializeField] private DialInteractable handleMoveWheels;
  
  [SerializeField] private XRSimpleInteractable shootButtonInteractable;
  [SerializeField] private XRSimpleInteractable playerEnterInteractable;
  [SerializeField] private XRSocketInteractor dropAreaSocket;

  [SerializeField] private GameObject cannon;
  [SerializeField] private GameObject cannonStructure;
  [SerializeField] private GameObject playerShootPos;
  [SerializeField] private GameObject objectShootPos;

  private XRBaseInteractable interactableInCannon;
  
  private float lastCannonRotation;
  private float lastWheelsRotation;
  private bool isShootCoroutineActive;
  private bool isPlayerInCannon;
  
  private void OnEnable()
  {
    handleRotateCannon.OnDialChanged.AddListener(RotateCannon);
    handleMoveWheels.OnDialChanged.AddListener(MoveWheels);
    
    shootButtonInteractable.activated.AddListener(ShootButtonActivated);
    playerEnterInteractable.selectEntered.AddListener(PutPlayerOntoCannon);
    
    dropAreaSocket.selectEntered.AddListener(DropAreaSelected);
  }

  private void PutPlayerOntoCannon(SelectEnterEventArgs arg0)
  {
    player.transform.position = playerShootPos.transform.position;
    player.transform.rotation = playerShootPos.transform.rotation;

    isPlayerInCannon = true;
  }

  private void OnDisable()
  {
    handleRotateCannon.OnDialChanged.RemoveListener(RotateCannon);
    handleMoveWheels.OnDialChanged.RemoveListener(MoveWheels);
    
    shootButtonInteractable.activated.RemoveListener(ShootButtonActivated);
    playerEnterInteractable.selectEntered.RemoveListener(PutPlayerOntoCannon);

    dropAreaSocket.selectEntered.RemoveListener(DropAreaSelected);
  }

  private void MoveWheels(DialInteractable arg0)
  {
    float wheelsDelta = arg0.CurrentAngle - lastWheelsRotation;

    if (wheelsDelta > 30 || wheelsDelta < -30)
      wheelsDelta = 0;
    
    transform.Translate(transform.right * wheelsDelta * 0.001f, Space.World);

    lastWheelsRotation = arg0.CurrentAngle;
    Debug.Log("Wheels" + arg0.CurrentAngle);
  }

  private void RotateCannon(DialInteractable arg0)
  {
    float cannonDelta = arg0.CurrentAngle - lastCannonRotation;
    
    if (cannonDelta > 30 || cannonDelta < -30)
      cannonDelta = 0;
    
    cannon.transform.Rotate(cannon.transform.up, cannonDelta * 0.2f, Space.World);
    
    lastCannonRotation = arg0.CurrentAngle;

    Debug.Log("Cannon" + arg0.CurrentAngle);
  }

  private void ShootButtonActivated(ActivateEventArgs arg0)
  {
    if (isShootCoroutineActive)
      return;
    
    Debug.Log("Shoot");

    StartCoroutine(ShootButtonCoroutine());
    // ShootObject();
  }

  private IEnumerator ShootButtonCoroutine()
  {
    isShootCoroutineActive = true;
    // Timer, show audio/UI
    
    yield return new WaitForSeconds(3f);

    if (isPlayerInCannon)
    {
      ShootPlayer();
    }
    else if (interactableInCannon)
    {
      ShootObject();
    }
    else
    {
      // Fail sound
    }
    
    
    StartCoroutine(DisableShootSocketAndReactivate());

    
    // After 3 seconds, decide
    
    // If there is nothing, dont shoot
    
    // If there is object, shoot object
    
    // If there is player, shoot player
    isShootCoroutineActive = false;
    isPlayerInCannon = false;
  }

  private void ShootPlayer()
  {
    // CREATE GO, ADD RB
    GameObject shootParentPlaceholder = new GameObject("ShootParent");
    
    // MAKE IT PARENT OF XRRIG
    shootParentPlaceholder.AddComponent<Rigidbody>();
    player.transform.parent = shootParentPlaceholder.transform;
    
    // SHOOT IT
    // player.GetComponent<CharacterController>().Move(playerShootPos.transform.forward * 10);

    shootParentPlaceholder.GetComponent<Rigidbody>().AddForce(objectShootPos.transform.forward * 1000);
  }

  private void ShootObject()
  {
    var firstInteractableSelected = dropAreaSocket.firstInteractableSelected;
    Debug.Log(firstInteractableSelected.transform.gameObject.name);

    // Disable object from socket
    interactionManager.CancelInteractorSelection(dropAreaSocket);
    
    // Shoot it with force
    firstInteractableSelected.transform.GetComponent<Rigidbody>().AddForce(objectShootPos.transform.forward * 1000);
  }

  private IEnumerator DisableShootSocketAndReactivate()
  {
    dropAreaSocket.enabled = false;

    yield return new WaitForSeconds(1f);
    dropAreaSocket.enabled = true;
  }


  private void DropAreaSelected(SelectEnterEventArgs arg0)
  {
    // If selected with an object, store the object
    interactableInCannon = (XRBaseInteractable) arg0.interactableObject;
  }
  
  // Things to do

  // Able to rotate itself if main Dial is moved

  // Able to rotate the cannon depending of input of middle handle

  // Able to move forwards or backwards with wheel handle
  
  // Shoot both interactables and player
  
  // Have a timer until it shoots
  
  


}
