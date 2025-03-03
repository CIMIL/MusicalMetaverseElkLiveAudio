using System;
using UnityEngine;
using UnityEngine.Events;

public class OctaveSelector : MonoBehaviour
{
    public int octave = 4;
    
    [NonSerialized]
    public UnityEvent OnOctaveChange;
    
    private void Awake()
    {
        OnOctaveChange = new UnityEvent();
    }
    
    public void NextOctave()
    {
        if (octave >= 10) return;
    
        octave++;
        OnOctaveChange.Invoke();
    }

    public void PreviousOctave()
    {
        if (octave <= 0) return;
    
        octave--;
        OnOctaveChange.Invoke();
    }
}
