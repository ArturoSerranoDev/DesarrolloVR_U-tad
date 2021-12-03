using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class HandHider : MonoBehaviour
{
    [SerializeField] private List<XRBaseInteractor> interactors;

    private void OnEnable()
    {
        foreach (XRBaseInteractor interactor in interactors)
        {
            interactor.selectEntered.AddListener(HideHand);
            interactor.selectExited.AddListener(ShowHand);
        }
    }

    private void ShowHand(SelectExitEventArgs exitEventArgs)
    {
        XRBaseInteractable grabInteractable = (XRBaseInteractable) exitEventArgs.interactableObject;
        this.gameObject.SetActive(true);
    }

    private void HideHand(SelectEnterEventArgs enterEventArgs)
    {
        this.gameObject.SetActive(false);
    }
}
