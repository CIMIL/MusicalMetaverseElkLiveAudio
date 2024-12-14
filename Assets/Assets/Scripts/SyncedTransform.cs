using MessageTypes;
using Ubiq.Messaging;
using UnityEngine;

public class SyncedTransform : MonoBehaviour
{
    private NetworkContext _context;
    private Vector3 _lastPosition;

    public void Start()
    {
        _context = NetworkScene.Register(this);
    }

    void Update()
    {
        if (_lastPosition != transform.position)
        {
            _lastPosition = transform.position;

            _context.SendJson(new Position()
            {
                position = transform.position,
                rotation = transform.rotation,
            });
        }
    }
    // TODO: Test if different components talk on different contexts altogether, thus removing the need for a messageType check
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        Message m = message.FromJson<Message>();
        if (m.messageType == MessageType.Position)
        {
            Position content = message.FromJson<Position>();
            transform.position = content.position;
            transform.rotation = content.rotation;
            _lastPosition = transform.position;
        }
    }

    public void SyncLastPosition()
    {
        _lastPosition = transform.position;
    }
}
