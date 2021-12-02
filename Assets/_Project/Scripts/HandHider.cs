using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHider : MonoBehaviour
{
    [SerializeField] private List<XRBaseInteractor> interactors;

    void Start()
    {
        
        foreach (var interactor in interactors)
        {
            interactor.selectEntered.AddListener(HideHand);
            interactor.selectExited.AddListener(ShowHand);
        }
    }

    private void HideHand(SelectEnterEventArgs arg0)
    {
        arg0.interactorObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    
    private void ShowHand(SelectExitEventArgs arg0)
    {
        arg0.interactorObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
