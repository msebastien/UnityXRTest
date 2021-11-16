using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class HandButton : XRBaseInteractable
{
    public UnityEvent OnPress = null;
    
    private float yMin = 0.0f; // Min Height (when pressed)
    private float yMax = 0.0f; // Max height (no press)
    private bool previousPress = false;

    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor;

    /// <summary>Init Event Listeners</summary> 
    protected override void Awake()
    {
        base.Awake();
        onHoverEnter.AddListener(StartPress);
        onHoverExit.AddListener(EndPress);
    }

    /// <summary>Destroy Event Listeners</summary> 
    private void OnDestroy()
    {
        onHoverEnter.RemoveListener(StartPress);
        onHoverExit.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        hoverInteractor = interactor;
        previousHandHeight = hoverInteractor.transform.position.y;
    }

    private void EndPress(XRBaseInteractor interactor)
    {
        // reset
        hoverInteractor = null;
        previousHandHeight = 0.0f;

        previousPress = false;
        SetYPosition(yMax); // return to inital state (button not pressed)
    }

    /// <summary>Init min and max positions</summary> 
    private void Start()
    {
        SetMinMax();
    }

    /// <summary>Check if the button is pressed</summary> 
    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f); // the button sinks halfway up the collider. 
        yMax = transform.localPosition.y; // current local position
    }

    /// <summary>Update button when there is an interactor (hand)</summary> 
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(hoverInteractor)
        {
            float newHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
            float handDifference = previousHandHeight - newHandHeight;
            previousHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    /// <summary>Get Local Y Position from a World Space position (Interactor/Hand)</summary> 
    public float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    /// <summary>Set the button vertical position (constrained by a min and max value)</summary> 
    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;
    }

    /// <summary>Check if the button is pressed</summary> 
    private void CheckPress()
    {
        bool inPosition = InPosition();

        if (inPosition && inPosition != previousPress)
            Debug.Log("Button Pressed");
            OnPress.Invoke();

        previousPress = inPosition;
    }

    /// <summary>Return if the button is in press position (Y minimum with a small range)</summary> 
    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);
        return transform.localPosition.y == inRange;
    }
}
