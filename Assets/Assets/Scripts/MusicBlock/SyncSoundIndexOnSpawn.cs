using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;

public class SyncSoundIndexOnSpawn : MonoBehaviour
{
    private NetworkContext context;
    private MusicBlock block;

    public void Start()
    {
        context = NetworkScene.Register(this);
        block = GetComponent<MusicBlock>();
    }

    public void SyncSound()
    {
        context.SendJson(new Message()
        {
            Index = block.soundIndex
        });
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
            Message content = message.FromJson<Message>();
            block.SetSoundIndex(content.Index);
    }
    
    private class Message
    {
        public int Index;
    }
}

