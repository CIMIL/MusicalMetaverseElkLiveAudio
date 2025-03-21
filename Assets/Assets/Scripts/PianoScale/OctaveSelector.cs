using System;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.Events;

public class OctaveSelector : MonoBehaviour
{
    public int octave = 4;
    
    [NonSerialized]
    public UnityEvent OnOctaveChange;

    private NetworkContext context;
    
    private void Awake()
    {
        OnOctaveChange = new UnityEvent();
        context = NetworkScene.Register(this);
    }
    
    public void NextOctave()
    {
        if (octave >= 10) return;
    
        octave++;
        context.SendJson(new Message() { Octave = octave });
        OnOctaveChange.Invoke();
    }

    public void PreviousOctave()
    {
        if (octave <= 0) return;
    
        octave--;
        OnOctaveChange.Invoke();
    }
    
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        octave = message.FromJson<Message>().Octave;
        OnOctaveChange.Invoke();
    }

    private struct Message
    {
        public int Octave;
    }
}
