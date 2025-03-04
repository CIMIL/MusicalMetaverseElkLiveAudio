using System;
using Ubiq.Messaging;
using UnityEngine;

public class SyncedTransform : MonoBehaviour
{
    private NetworkContext context;
    private Vector3 lastPosition;
    
    [NonSerialized]
    public float Velocity = 0;

    public void Start()
    {
        context = NetworkScene.Register(this);
    }

    private void Update()
    {
        // Note: Velocity is only calculated when a movement occurs, i.e. it is never zero
        if (lastPosition != transform.position)
        {
            Velocity = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
            lastPosition = transform.position;
            context.SendJson(new Message()
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Velocity = Velocity
            });
        }
        
        
    }
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        Message content = message.FromJson<Message>();
        transform.position = content.Position;
        transform.rotation = content.Rotation;
        Velocity = content.Velocity;
        
        lastPosition = transform.position;
    }

    public void SyncLastPosition()
    {
        lastPosition = transform.position;
    }

    private class Message
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float Velocity;
    }
}
