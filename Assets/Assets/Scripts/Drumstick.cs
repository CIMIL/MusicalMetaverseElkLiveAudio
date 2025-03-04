using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Drumstick : MonoBehaviour
{
    
    private XRGrabInteractable grabInteractable;
    public NetworkId grabbedBy;
    private NetworkContext context;
    
    public void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
        
        context = NetworkScene.Register(this);
    }
    
    private struct Message
    {
        public NetworkId GrabbedBy;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        grabbedBy = NetworkScene.Find(this).Id;
        SendUpdateMessage();
    }
    
    private void OnSelectExited(SelectExitEventArgs args)
    {
        grabbedBy = NetworkId.Null;
        SendUpdateMessage();
    }

    public void SendUpdateMessage()
    {
        context.SendJson(new Message()
        {
            GrabbedBy = this.grabbedBy
        });
    }
    
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        Message m = message.FromJson<Message>();
        grabbedBy = m.GrabbedBy;
    }

    
}
