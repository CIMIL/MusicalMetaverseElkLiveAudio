using TMPro;
using UnityEngine;

public class PresetSelector : MonoBehaviour
{
    [SerializeField]
    private PresetList presets;
    [SerializeField]
    private BlockGroup blockGroup;
    [SerializeField]
    private TMP_Text panelText;
    
    private int presetIndex = 0;

    private void Start()
    {
        ApplyPreset();
    }
    
    public void NextPreset()
    {
        presetIndex = (presetIndex + 1) % presets.Count();
        ApplyPreset();
    }

    public void PreviousPreset()
    {
        presetIndex = (presetIndex - 1) % presets.Count();
        ApplyPreset();
    }

    private void ApplyPreset()
    {
        panelText.text = presets.Get(presetIndex).Item1;
        blockGroup.SetPreset(presets.Get(presetIndex).Item2);
    }

    public string GetActivePreset()
    {
        return presets.Get(presetIndex).Item1;
    }
}
