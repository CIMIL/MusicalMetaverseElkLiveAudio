using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TinyJson;
using Ubiq.Avatars;
using Ubiq.Logging;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.Events;
using Avatar = Ubiq.Avatars.Avatar;

public class MainCameraLogEmitter : MonoBehaviour
{

    public AvatarManager avatarManager;
    [SerializeField] private string headTag = "Player Head";
    [SerializeField] private int raycastDistance = 25;
    
    private LogEmitter logEmitter;
    private bool lookingAt = false;
    private NetworkId lookingAtNetworkId;
    private NetworkContext context;
    private NetworkId myNetworkSceneId;
    private void Start()
    {
        logEmitter = new InfoLogEmitter(this);
        InvokeRepeating(nameof(LogVector), 0.0f, 0.3f);
        context = NetworkScene.Register(this);
        myNetworkSceneId = NetworkScene.Find(this).Id;
        avatarManager = GameObject.Find("Avatar Manager").GetComponent<AvatarManager>();
    }
    
    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, raycastDistance) && 
            hit.collider.gameObject.CompareTag(headTag))
        {
            if (lookingAt) return;
            lookingAt = true;
            
            context.SendJson(new Message()
            {
                sourceId = myNetworkSceneId,
                avatarId = hit.collider.gameObject.transform.parent.GetComponentInParent<Avatar>().NetworkId,
                targetId = NetworkId.Null
            });
        }
        else if (lookingAt)
        {
            lookingAt = false;
            lookingAtNetworkId = NetworkId.Null;
            logEmitter.Log("Stopped looking at ", lookingAtNetworkId);
        }
    }
    
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var m = message.FromJson<Message>();
        
        if (m.sourceId == myNetworkSceneId)
        {
            logEmitter.Log("Is looking at ", m.targetId);
            lookingAtNetworkId = m.targetId;
        } else if (m.avatarId == avatarManager.LocalAvatar.NetworkId)
        {
            context.SendJson(new Message()
            {
                sourceId = m.sourceId,
                avatarId = m.avatarId,
                targetId = myNetworkSceneId
            });
        }
    }
    
    private class Message
    {
        public NetworkId sourceId;
        public NetworkId avatarId;
        public NetworkId targetId;
    }
    
    private void LogVector()
    {
        logEmitter.Log("Camera position and direction", transform.forward, transform.position);
    }
}
