using System;
using extOSC;
using Ubiq.Messaging;
using Ubiq.XRI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BlockGroup : MonoBehaviour
{
    public bool spectatorMode = false;
    
    [SerializeField]
    private OSCTransmitter transmitter;
    
    void Start()
    {
        transmitter = FindObjectOfType<OSCTransmitter>();
    }

    public void Disable()
    {
        spectatorMode = true;

        // Disable logging for interactions triggered by the player on other player's pianos
        foreach (BlockInteractionLogger blockLogger in GetComponentsInChildren<BlockInteractionLogger>())
        {
            blockLogger.enabled = false;   
        }

        OctaveSelector octaveSelector = GetComponentInChildren<OctaveSelector>();
        octaveSelector.GetComponent<MenuAdapterXRI>().enabled = false;
        octaveSelector.transform.GetChild(0).gameObject.SetActive(false);
        
        PresetSelector presetSelector = GetComponentInChildren<PresetSelector>();
        presetSelector.GetComponent<MenuAdapterXRI>().enabled = false;
        presetSelector.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SendNote(bool play, int note)
    {
        if (spectatorMode)
            return;
        
        var message = new OSCMessage("/keyboard_event/x_source");
        message.AddValue(OSCValue.String(play ? "note_on" : "note_off"));
        message.AddValue(OSCValue.Int(0));
        message.AddValue(OSCValue.Int(note));
        message.AddValue(OSCValue.Float(1.0f));

        transmitter.Send(message);
    }

    public void SetVibrato(float normalizedValue)
    {
        var message = new OSCMessage("/parameter/x_mda_jx10/Vibrato_");
        message.AddValue(OSCValue.Float(normalizedValue));
        
        transmitter.Send(message);
    }

    public void SetPreset(int id)
    {
        var message = new OSCMessage("/program/x_mda_jx10");
        message.AddValue(OSCValue.Int(id));
        
        transmitter.Send(message);
    }
    
}
