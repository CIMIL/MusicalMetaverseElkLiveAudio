using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class ButtonFollowVisual : MonoBehaviour
{
    public float resetSpeed = 5;
    
    [SerializeField] private Transform visualTarget;
    [SerializeField] private Vector3 localAxis;

    private float followAngleThreshold;
    private Vector3 initialLocalPosition;
    private Vector3 offset;
    private Transform pokeAttachTransform;
    private XRBaseInteractable interactable;
    private bool isFollowing = false;
    private bool freeze = false;
    
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(ResetPosition);
        interactable.selectEntered.AddListener(Freeze);

        followAngleThreshold = GetComponent<XRPokeFilter>().pokeConfiguration.Value.pokeAngleThreshold;
        initialLocalPosition = visualTarget.localPosition;
    }

    private void Follow(HoverEnterEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor interactor)
        {
            
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;       

            var pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));
            if (pokeAngle < followAngleThreshold)
            {
                isFollowing = true;
                freeze = false;
            }
        }
    }

    private void ResetPosition(HoverExitEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }

    private void Freeze(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor)
        {
            freeze = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (freeze)
            return;
        
        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPosition, Time.deltaTime * resetSpeed);
        }
    }
}
