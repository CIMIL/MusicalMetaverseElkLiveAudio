using System;
using extOSC;
using Ubiq.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BlockGroup : MonoBehaviour
{
    [SerializeField]
    private OSCTransmitter transmitter;
    
    void Start()
    {
        transmitter = FindObjectOfType<OSCTransmitter>();
    }

    public void Disable()
    {
        GetComponent<OSCTransmitter>().enabled = false;
        
        var colliders = GetComponentsInChildren<BoxCollider>();
        foreach (var boxCollider in colliders)
           boxCollider.enabled = false;
        
        GetComponentInChildren<OctaveSelector>().gameObject.SetActive(false);
        GetComponentInChildren<PresetSelector>().gameObject.SetActive(false);
        
    }

    public void SendNote(bool play, int note)
    {
        var message = new OSCMessage("/keyboard_event/x_source");
        message.AddValue(OSCValue.String(play ? "note_on" : "note_off"));
        message.AddValue(OSCValue.Int(0));
        message.AddValue(OSCValue.Int(note));
        message.AddValue(OSCValue.Float(1.0f));

        transmitter.Send(message);
    }

    public void SetVibrato(float normalizedValue)
    {
        var message = new OSCMessage("/parameter/x_Mda_jx10/Vibrato_");
        message.AddValue(OSCValue.Float(normalizedValue));
        
        transmitter.Send(message);
    }

    public void SetPreset(int id)
    {
        var message = new OSCMessage("/program/x_Mda_jx10");
        message.AddValue(OSCValue.Int(id));
        
        transmitter.Send(message);
    }
    
}
