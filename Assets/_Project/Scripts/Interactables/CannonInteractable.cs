using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CannonInteractable : MonoBehaviour
{
  // VARIABLES
  [SerializeField] private DialInteractable handleRotateFullCannon;
  [SerializeField] private DialInteractable handleRotateCannon;
  [SerializeField] private DialInteractable handleMoveWheels;
  
  [SerializeField] private XRSimpleInteractable shootButtonInteractable;

  [SerializeField] private GameObject cannon;
  [SerializeField] private GameObject cannonStructure;
  [SerializeField] private GameObject playerShootPos;
  [SerializeField] private GameObject objectShootPos;

  private XROrigin playerInCannon;
  private XRBaseInteractable interactableInCannon;
  private void OnEnable()
  {
    handleRotateFullCannon.OnDialChanged.AddListener(RotateFullCannon);
    handleRotateCannon.OnDialChanged.AddListener(RotateCannon);
    handleMoveWheels.OnDialChanged.AddListener(MoveWheels);
    
    shootButtonInteractable.activated.AddListener(ShootButtonActivated);
  }

  private void OnDisable()
  {
    handleRotateFullCannon.OnDialChanged.RemoveListener(RotateFullCannon);
    handleRotateCannon.OnDialChanged.RemoveListener(RotateCannon);
    handleMoveWheels.OnDialChanged.RemoveListener(MoveWheels);
    
    shootButtonInteractable.activated.RemoveListener(ShootButtonActivated);
  }

  private void MoveWheels(DialInteractable arg0)
  {
    
  }

  private void RotateCannon(DialInteractable arg0)
  {
    
  }

  private void RotateFullCannon(DialInteractable arg0)
  {
    
  }

  private void ShootButtonActivated(ActivateEventArgs arg0)
  {
    
  }

  // Things to do

  // Able to rotate itself if main Dial is moved

  // Able to rotate the cannon depending of input of middle handle

  // Able to move forwards or backwards with wheel handle
  
  // Shoot both interactables and player
  
  // Have a timer until it shoots
  
  


}
