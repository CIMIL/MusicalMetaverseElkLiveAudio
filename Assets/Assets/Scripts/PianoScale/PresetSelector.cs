using TMPro;
using Ubiq.Logging;
using UnityEngine;

public class PresetSelector : MonoBehaviour
{
    
    [SerializeField] private PresetList presets;
    [SerializeField] private BlockGroup blockGroup;
    [SerializeField] private TMP_Text panelText;
    [SerializeField] private bool logging;
    
    private int presetIndex = 0;
    private LogEmitter logEmitter;

    private void Start()
    {
        ApplyPreset();

        if (logging)
            logEmitter = new ExperimentLogEmitter(this);
    }
    
    public void NextPreset()
    {
        presetIndex = (presetIndex + 1) % presets.Count();
        Debug.Log(presetIndex);
        ApplyPreset();

        if (logging)
            Log();
    }

    public void PreviousPreset()
    {
        presetIndex = --presetIndex < 0 ? presets.Count() + presetIndex : presetIndex;
        Debug.Log(presetIndex);
        ApplyPreset();
        
        if (logging)
            Log();
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

    private void Log()
    {
        PresetData p = new PresetData(presetIndex, presets.Get(presetIndex).Item1);
        logEmitter.Log("Preset Changed", p);
    }

    private struct PresetData
    {
        public int PresetIndex;
        public string PresetName;

        public PresetData(int presetIndex, string presetName)
        {
            PresetIndex = presetIndex;
            PresetName = presetName;
        }
    }
}
