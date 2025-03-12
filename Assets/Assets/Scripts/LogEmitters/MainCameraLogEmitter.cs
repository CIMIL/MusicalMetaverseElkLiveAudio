using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TinyJson;
using Ubiq.Avatars;
using Ubiq.Logging;
using Ubiq.Messaging;
using Ubiq.Rooms;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Events;
using Avatar = Ubiq.Avatars.Avatar;

public class MainCameraLogEmitter : MonoBehaviour
{
    
    [SerializeField] private string headTag = "Player Head";
    [SerializeField] private int raycastDistance = 25;
    
    private LogEmitter logEmitter;
    
    private bool lookingAt;
    private string lookingAtPeerId;
    
    private struct MainCameraEvent
    {
        public Vector3 Forward;
        public Vector3 Position;

        public MainCameraEvent(Vector3 forward, Vector3 position)
        {
            this.Forward = forward;
            this.Position = position;
        }
    }
    
    private struct LookingAtEvent
    {
        public string Type;
        public string PeerId;

        public LookingAtEvent(string type, string peerId)
        {
            this.Type = type;
            this.PeerId = peerId;
        }
    }

    public void Start()
    {
        logEmitter = new InfoLogEmitter(this);
        InvokeRepeating(nameof(LogVector), 0.0f, 0.3f);
    }
    
    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, raycastDistance) && 
            hit.collider.gameObject.CompareTag(headTag))
        {
            if (lookingAt) return;
            
            lookingAt = true;
            lookingAtPeerId = hit.collider.GetComponentInParent<Avatar>().Peer.networkId.ToString();
            logEmitter.Log("Looking At", new LookingAtEvent("Started", lookingAtPeerId));
            
        }
        else if (lookingAt)
        {
            logEmitter.Log("Looking At", new LookingAtEvent("Stopped", lookingAtPeerId));
            lookingAt = false;
        }
    }
    
    private void LogVector()
    {
        logEmitter.Log("Main Camera", new MainCameraEvent(transform.forward, transform.position));
    }
}