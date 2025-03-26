using System;
using Ubiq.Messaging;
using Ubiq.Spawning;
using UnityEngine;
using UnityEngine.Events;

public class OctaveSelector : MonoBehaviour, INetworkSpawnable
{
    public int octave = 4;
    
    [NonSerialized]
    public UnityEvent OnOctaveChange;

    public NetworkId NetworkId { get; set; }
    private NetworkContext context;
    
    private void Awake()
    {
        OnOctaveChange = new UnityEvent();
    }

    private void Start()
    {
        NetworkId = NetworkId.Create(this);
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
        context.SendJson(new Message() { Octave = octave });
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
