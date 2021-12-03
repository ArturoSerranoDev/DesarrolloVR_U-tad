using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class TeleportFadeEffect : MonoBehaviour
{
    List<BaseTeleportationInteractable> teleportationInteractables;

    [SerializeField] private Volume fadeToBlackVolume;

    // Start is called before the first frame update
    void Awake()
    {
        teleportationInteractables = FindObjectsOfType<BaseTeleportationInteractable>().ToList();
    }

    private void OnEnable()
    {
        foreach (var teleportInteractable in teleportationInteractables)
        {
            teleportInteractable.teleporting.AddListener(FadeToBlack);
        }
    }
    private void OnDisable()
    {
        foreach (var teleportInteractable in teleportationInteractables)
        {
            teleportInteractable.teleporting.RemoveListener(FadeToBlack);
        }
    }

    private void FadeToBlack(TeleportingEventArgs arg0)
    {
        DOTween.To(() => fadeToBlackVolume.weight, x => fadeToBlackVolume.weight = x, 1f, 1).OnComplete(() =>
        {
            DOTween.To(() => fadeToBlackVolume.weight, x => fadeToBlackVolume.weight = x, 0f, 1);
        });
    }
}
