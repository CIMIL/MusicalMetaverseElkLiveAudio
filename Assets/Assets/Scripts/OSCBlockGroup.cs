using extOSC;
using Ubiq.Messaging;
using UnityEngine;

public class OSCBlockGroup : MonoBehaviour
{

    private OSCTransmitter transmitter;
    private NetworkContext context;

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        transmitter = GetComponent<OSCTransmitter>();
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
    }

    public void Send(int soundIndex, bool active)
    {
        //client.Send("/instruments", string.Format("({0}, {1}, {2})", context.Id.ToString() ,soundIndex.ToString(), active))
        
        var message = new OSCMessage("/keyboard_event/x_source");
        message.AddValue(OSCValue.String(active ? "note_on" : "note_off"));
        message.AddValue(OSCValue.Int(0));
        message.AddValue(OSCValue.Int(60));
        message.AddValue(OSCValue.Float(1.0f));

        transmitter.Send(message);
    }
}
