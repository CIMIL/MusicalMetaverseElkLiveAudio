using System;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class InstrumentBlock : MonoBehaviour
{
    private static readonly int BaseColor = Shader.PropertyToID("_Base_Color");
    private static readonly int EdgeThreshold = Shader.PropertyToID("_Edge_Threshold");

    public string interactableTag;
    public int note {get; private set;}
    
    [SerializeField] private Material cubeMaterial;
    [SerializeField] private int sequenceNumber;
    [SerializeField] private BlockGroup blockGroup;
    [SerializeField] private OctaveSelector octaveSelector;
    [SerializeField] private float rateOfChange;
    [SerializeField] private float zeroVelocity;
    [SerializeField] private float oneVelocity;
    [SerializeField] private float zeroEdgeThreshold;
    [SerializeField] private float oneEdgeThreshold;
    
    private Color baseColor;
    private Color pressedColor;
    private MeshRenderer meshRenderer;
    private bool pressed = false;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        SetOctave();
        octaveSelector.OnOctaveChange.AddListener(SetOctave);
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            pressed = true;
            StartCoroutine(SmoothVibrato(other.gameObject));
            blockGroup.SendNote(true, note);
            Debug.Log(note);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            pressed = false;
            ApplyColor(baseColor, zeroEdgeThreshold);
            blockGroup.SetVibrato(0f);
            blockGroup.SendNote(false, note);
        }
    }
    
    private IEnumerator SmoothVibrato(GameObject drumstick)
    {
        var drumstickTransform = drumstick.GetComponentInParent<SyncedTransform>();
        var smoothedVelocity = 0f;

        while (pressed)
        {
            var mappedVelocity = Mathf.InverseLerp(zeroVelocity, oneVelocity, drumstickTransform.Velocity);
            var deltaVelocity = mappedVelocity - smoothedVelocity;

            if (Math.Abs(deltaVelocity) > rateOfChange * Time.deltaTime)
            {
                var nextVelocity = smoothedVelocity + rateOfChange * Time.deltaTime * Math.Sign(deltaVelocity);
                smoothedVelocity = Math.Clamp(nextVelocity, 0f, 1f);
            }
            else
            {
                smoothedVelocity = Math.Clamp(smoothedVelocity + deltaVelocity, 0f, 1f);
            }
            
            var edgeThreshold = Mathf.Lerp(zeroEdgeThreshold, oneEdgeThreshold, smoothedVelocity);
            blockGroup.SetVibrato(smoothedVelocity);
            ApplyColor(pressedColor, edgeThreshold);
            
            yield return null;
        }
    }

    private void CalculateColor()
    {
        var noteColorOffset = 1f / (12f * 11f);
        
        var h = noteColorOffset * note;
        var v = 1f;
        var vPressed = 0.75f;
        
        if (sequenceNumber <= 4 && sequenceNumber % 2 == 1 || 
            sequenceNumber > 4 && sequenceNumber % 2 == 0)
        {
            v = 0f;
            vPressed = 0.25f;
        }
        
        baseColor = Color.HSVToRGB(h, 0.5f, v);
        pressedColor = Color.HSVToRGB(h, 0.5f, vPressed);
    }
    
    private void ApplyColor(Color color, float edgeThreshold)
    {
        var propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor(BaseColor, color);
        propertyBlock.SetFloat(EdgeThreshold, edgeThreshold);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    private void SetOctave()
    {
        note = octaveSelector.octave * 11 + sequenceNumber;
        CalculateColor();
        ApplyColor(baseColor, zeroEdgeThreshold);
    }
    
}
