using extOSC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OSCPanel : MonoBehaviour
{
    public TMP_Text textfield;
    private OSCReceiver receiver;

    private void Start()
    {
        receiver = GetComponent<OSCReceiver>();
        receiver.Bind("/keyboard_event/x_source", OnMessageReceive);
    }
        
    public void OnMessageReceive(OSCMessage message)
    {
        UnityMainThread.wkr.AddJob(() =>
        {
            textfield.text = message.Values[0].StringValue;
        });
        
    }
}
