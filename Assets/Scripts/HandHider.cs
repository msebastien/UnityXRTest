using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHider : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer = null;
    private XRDirectInteractor interactor = null;

    private void Awake()
    {
        interactor = GetComponent<XRDirectInteractor>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        interactor.onHoverEnter.AddListener(Hide);
        interactor.onHoverExit.AddListener(Show);
    }

    private void OnDestroy()
    {
        interactor.onHoverEnter.RemoveListener(Hide);
        interactor.onHoverExit.RemoveListener(Show);
    }

    private void Show(XRBaseInteractable obj)
    {
        meshRenderer.enabled = true;
    }

    private void Hide(XRBaseInteractable obj)
    {
        meshRenderer.enabled = false;
    }
}
