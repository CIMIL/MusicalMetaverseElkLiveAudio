using System;
using Ubiq.Logging;
using Ubiq.Messaging;
using Ubiq.Spawning;
using UnityEngine;
using UnityEngine.Events;

public class OctaveSelector : MonoBehaviour, INetworkSpawnable
{
    public NetworkId NetworkId { get; set; }
    public int octave = 4;
    
    [NonSerialized]
    public UnityEvent OnOctaveChange;

    [SerializeField] private bool logging;
    
    private LogEmitter logEmitter;
    private NetworkContext context;
    
    private void Awake()
    {
        OnOctaveChange = new UnityEvent();
    }

    private void Start()
    {
        NetworkId = NetworkId.Create(this);
        context = NetworkScene.Register(this);
        
        if (logging)
            logEmitter = new ExperimentLogEmitter(this);
    }
    
    public void NextOctave()
    {
        if (octave >= 10) return;
    
        octave++;
        context.SendJson(new Message() { Octave = octave });
        OnOctaveChange.Invoke();
        
        if(logging)
            Log();
    }

    public void PreviousOctave()
    {
        if (octave <= 0) return;
    
        octave--;
        context.SendJson(new Message() { Octave = octave });
        OnOctaveChange.Invoke();

        if (logging)
            Log();
    }
    
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        octave = message.FromJson<Message>().Octave;
        OnOctaveChange.Invoke();
    }

    private void Log()
    {
        var octaveData = new OctaveData(octave);
        logEmitter.Log("Octave Changed", octaveData);
    }

    private struct Message
    {
        public int Octave;
    }

    private struct OctaveData
    {
        public int Octave;

        public OctaveData(int octave)
        {
            Octave = octave;
        }
    }
}
