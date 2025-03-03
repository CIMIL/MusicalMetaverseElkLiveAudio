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
    
    private int Note;
    private Color baseColor;
    private Color pressedColor;
    
    private void Start()
    {
        SetOctave();
        octaveSelector.OnOctaveChange.AddListener(SetOctave);
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            ApplyColor(pressedColor);
            blockGroup.SendNote(true, Note);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            ApplyColor(baseColor);
            blockGroup.SendNote(false, Note);
        }
    }

    private void CalculateColor()
    {
        float noteColorOffset = 1f / (12f * 11f);
        
        float h = noteColorOffset * Note;
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
    
    private void ApplyColor(Color color)
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor(BaseColor, color);
        GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
    }

    private void SetOctave()
    {
        Note = octaveSelector.octave * 11 + sequenceNumber;
        CalculateColor();
        ApplyColor(baseColor);
    }
    
}
