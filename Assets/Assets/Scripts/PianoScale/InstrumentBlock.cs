using System;
using Unity.VisualScripting;
using UnityEngine;

public class InstrumentBlock : MonoBehaviour
{
    private static readonly int BaseColor = Shader.PropertyToID("_Base_Color");
    private static readonly int EdgeThreshold = Shader.PropertyToID("_Edge_Threshold");
    private static readonly int GlowColor = Shader.PropertyToID("_Glow_Color");
    private static readonly int IntersectionPower = Shader.PropertyToID("_Intersection_Power");

    public string interactableTag;
    
    [SerializeField]
    private Material cubeMaterial;
    
    [SerializeField]
    private int sequenceNumber;
    
    [SerializeField]
    private BlockGroup blockGroup;
    
    [SerializeField]
    private OctaveSelector octaveSelector;
    
    private int note;
    private Color baseColor;
    private Color pressedColor;
    private bool pressed = false;
    private SyncedTransform drumstickTransform;
    
    private void Start()
    {
        SetOctave();
        octaveSelector.OnOctaveChange.AddListener(SetOctave);
    }

    private void Update()
    {
        if (pressed)
        {
            float velocity = drumstickTransform.Velocity;
            float mappedVelocity = Mathf.InverseLerp(0f, 3f, velocity);
            float lerpedEdgeThreshold = Mathf.Lerp(0.02f, 0.25f, mappedVelocity);
            
            blockGroup.SetVibrato(mappedVelocity);
            ApplyColor(pressedColor, lerpedEdgeThreshold);
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            pressed = true;
            drumstickTransform = other.gameObject.GetComponentInParent<SyncedTransform>();
            
            ApplyColor(pressedColor);
            blockGroup.SendNote(true, note);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            pressed = false;
            ApplyColor(baseColor);
            blockGroup.SetVibrato(0f);
            blockGroup.SendNote(false, note);
        }
    }

    private void CalculateColor()
    {
        float noteColorOffset = 1f / (12f * 11f);
        
        float h = noteColorOffset * note;
        float v = 1f;
        float vPressed = 0.75f;
        
        if (sequenceNumber <= 4 && sequenceNumber % 2 == 1 || 
            sequenceNumber > 4 && sequenceNumber % 2 == 0)
        {
            v = 0f;
            vPressed = 0.25f;
        }
        
        baseColor = Color.HSVToRGB(h, 0.5f, v);
        pressedColor = Color.HSVToRGB(h, 0.5f, vPressed);
    }
    
    private void ApplyColor(Color color, float edgeThreshold = 0.02f)
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor(BaseColor, color);
        propertyBlock.SetFloat(EdgeThreshold, edgeThreshold);
        GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
    }

    private void SetOctave()
    {
        note = octaveSelector.octave * 11 + sequenceNumber;
        CalculateColor();
        ApplyColor(baseColor);
    }
    
}
