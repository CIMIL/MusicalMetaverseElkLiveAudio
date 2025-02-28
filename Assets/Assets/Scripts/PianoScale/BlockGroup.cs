using extOSC;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BlockGroup : MonoBehaviour
{

    private OSCTransmitter transmitter;
    public int octave = 0;
    public UnityEvent onOctaveChange;

    // Start is called before the first frame update
    
    private void Awake()
    {
        onOctaveChange = new UnityEvent();
    }
    void Start()
    {
        transmitter = GetComponent<OSCTransmitter>();
    }

    public void Disable()
    {
        GetComponent<OSCTransmitter>().enabled = false;
        
        var colliders = GetComponentsInChildren<BoxCollider>();
        foreach (var boxCollider in colliders)
           boxCollider.enabled = false;
        
    }

    public void Send(bool play, int note)
    {
        //client.Send("/instruments", string.Format("({0}, {1}, {2})", context.Id.ToString() ,soundIndex.ToString(), active))
        
        var message = new OSCMessage("/keyboard_event/x_source");
        message.AddValue(OSCValue.String(play ? "note_on" : "note_off"));
        message.AddValue(OSCValue.Int(0));
        message.AddValue(OSCValue.Int(note));
        message.AddValue(OSCValue.Float(1.0f));

        transmitter.Send(message);
    }
    
    public void NextOctave()
    {
        if (octave >= 10) return;
        
        octave++;
        onOctaveChange.Invoke();
    }

    public void PreviousOctave()
    {
        if (octave <= 0) return;
        
        octave--;
        onOctaveChange.Invoke();
    }

    public void NextInstrument()
    {
        
    }

    public void PreviousInstrument()
    {
        
    }
}
