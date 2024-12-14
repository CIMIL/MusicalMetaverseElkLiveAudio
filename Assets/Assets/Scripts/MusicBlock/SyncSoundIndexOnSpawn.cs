using MessageTypes;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;

public class SyncSoundIndexOnSpawn : MonoBehaviour
{
    public NetworkContext context;
    private MusicBlock block;

    public void Start()
    {
        context = NetworkScene.Register(this);
        block = GetComponent<MusicBlock>();
    }

    public void SyncSound()
    {
        context.SendJson(new SoundData()
        {
            index = block.soundIndex
        });
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        Message m = message.FromJson<Message>();

        if (m.messageType == MessageType.SoundData)
        {
            SoundData content = message.FromJson<SoundData>();
            block.SetSoundIndex(content.index);
        }
    }
}
