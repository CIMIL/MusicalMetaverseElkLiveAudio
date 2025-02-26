using UnityEngine;

public class InstrumentBlock : MonoBehaviour
{
    private static readonly int BaseColor = Shader.PropertyToID("_Base_Color");
    private static readonly int EdgeThreshold = Shader.PropertyToID("_Edge_Threshold");
    private static readonly int GlowColor = Shader.PropertyToID("_Glow_Color");
    private static readonly int IntersectionPower = Shader.PropertyToID("_Intersection_Power");

    public Material cubeMaterial;
    public string interactableTag;
    public int sequenceNumber;
    public BlockGroup blockGroup;
    
    private Color baseColor;
    private Color pressedColor;
    
    private void Start()
    {
        CalculateColor();
        ApplyColor(baseColor);
        
        blockGroup.onOctaveChange.AddListener(() =>
        {
            CalculateColor();
            ApplyColor(baseColor);
        });
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            ApplyColor(pressedColor);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            ApplyColor(baseColor);
        }
    }

    private void CalculateColor()
    {
        float octaveColorSize = 1f / 11f;
        float octaveOffset = octaveColorSize * blockGroup.octave;
        float h = octaveOffset + (octaveColorSize / 12f) * sequenceNumber;
        
        baseColor = Color.HSVToRGB(h, 1f, 1f);
        pressedColor = Color.HSVToRGB(h, 1f, 0.75f);
    }

    private void ApplyColor(Color color)
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor(BaseColor, color);
        GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
    }
    
}
