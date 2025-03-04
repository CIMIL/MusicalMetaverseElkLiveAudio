using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;
using UnityEngine.UI;

public class GainController : MonoBehaviour
{

    [SerializeField]
    private OSCTransmitter transmitter;
    [SerializeField]
    private Slider yourGainSlider;
    [SerializeField]
    private Slider othersGainSlider;

    public void Start()
    {
        SetYourGain();
        SetOthersGain();
    }

    public void SetYourGain()
    {
        var message = new OSCMessage("/parameter/x_cimil_track_send_own/gain");
        message.AddValue(OSCValue.Float(yourGainSlider.value));
        transmitter.Send(message);
        Debug.Log(yourGainSlider.value);
    }

    public void SetOthersGain()
    {
        // TODO: Change OSC address to the other gain parameter
        var message = new OSCMessage("/parameter/master_send_own/gain");
        message.AddValue(OSCValue.Float(othersGainSlider.value));
        transmitter.Send(message);
        Debug.Log(othersGainSlider.value);
    }
}
