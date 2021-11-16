using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ColorChanger : MonoBehaviour
{
    [Tooltip("Item color when it is not being grabbed.")]
    public Material idleItemMaterial = null;
    [Tooltip("Item color when grabbed.")]
    public Material grabbedItemMaterial = null;

    private MeshRenderer meshRenderer = null;
    private XRGrabInteractable grabInteractable = null;

    public void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onActivate.AddListener(SetGrabbedMaterial);
        grabInteractable.onDeactivate.AddListener(SetIdleMaterial);
    }

    public void OnDestroy()
    {
        grabInteractable.onActivate.RemoveListener(SetGrabbedMaterial);
        grabInteractable.onDeactivate.RemoveListener(SetIdleMaterial);
    }

    public void SetGrabbedMaterial(XRBaseInteractor obj)
    {
        meshRenderer.material = grabbedItemMaterial;
    }

    public void SetIdleMaterial(XRBaseInteractor obj)
    {
        meshRenderer.material = idleItemMaterial;
    }

}
