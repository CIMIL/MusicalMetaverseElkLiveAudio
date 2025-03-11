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

    [SerializeField] private AvatarManager avatarManager;
    [SerializeField] private string headTag = "Player Head";
    [SerializeField] private int raycastDistance = 25;
    
    private LogEmitter logEmitter;
    private bool lookingAt;
    private bool waitingForResponse;
    private NetworkContext context;
    private NetworkId myNetworkSceneId;
    private int lookingAtEventCounter = 0;
    private void Start()
    {
        logEmitter = new InfoLogEmitter(this);
        context = NetworkScene.Register(this);
        myNetworkSceneId = NetworkScene.Find(this).Id;
        avatarManager = GameObject.Find("Avatar Manager").GetComponent<AvatarManager>();
        InvokeRepeating(nameof(LogVector), 0.0f, 0.3f);
    }
    
    private void Update()
    {
        
        if (Physics.Raycast(transform.position, transform.forward, out var hit, raycastDistance) && 
            hit.collider.gameObject.CompareTag(headTag))
        {
            if (lookingAt) return;
            
            lookingAt = true;
            logEmitter.Log("Looking At", "Started" , lookingAtEventCounter);
            
            context.SendJson(new Message()
            {
                SourceId = myNetworkSceneId,
                AvatarId = hit.collider.gameObject.GetComponentInParent<Avatar>().NetworkId,
                TargetId = NetworkId.Null,
                LookingAtEventCounter = lookingAtEventCounter
            });
            
        }
        else if (lookingAt)
        {
            logEmitter.Log("Looking At", "Stopped", lookingAtEventCounter);
            lookingAt = false;
            lookingAtEventCounter++;
        }
    }
    
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var m = message.FromJson<Message>();
        
        if (m.SourceId == myNetworkSceneId)
        {
            logEmitter.Log("Looking At", "Resolved", m.LookingAtEventCounter, m.TargetId);
        } else if (m.AvatarId == avatarManager.LocalAvatar.NetworkId)
        {
            context.SendJson(new Message()
            {
                SourceId = m.SourceId,
                AvatarId = m.AvatarId,
                TargetId = myNetworkSceneId,
                LookingAtEventCounter = m.LookingAtEventCounter
            });
        }
    }
    
    private class Message
    {
        public NetworkId SourceId;
        public NetworkId AvatarId;
        public NetworkId TargetId;
        public int LookingAtEventCounter;
    }
    
    private void LogVector()
    {
        logEmitter.Log("Main Camera", transform.forward, transform.position);
    }
}
