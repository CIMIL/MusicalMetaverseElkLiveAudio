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

    public void Start()
    {
        SetYourGain();
    }

    public void SetYourGain()
    {
        var message = new OSCMessage("/parameter/x_cimil_track_send_own/gain");
        message.AddValue(OSCValue.Float(yourGainSlider.value));
        transmitter.Send(message);
    }
}
