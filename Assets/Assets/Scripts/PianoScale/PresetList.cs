using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PresetList")]
public class PresetList : ScriptableObject
{ 
    public List<string> instruments;
    public List<int> instrumentIds;

    private void OnEnable()
    {
        if (instruments.Count != instrumentIds.Count)
            throw new ArgumentException("Instruments and instrument IDs do not match");
    }
    
    public Tuple<string, int> Get(int index)
    {
        return new Tuple<string, int>(instruments[index], instrumentIds[index]);
    }

    public int Count()
    {
        return instruments.Count;
    }
}
