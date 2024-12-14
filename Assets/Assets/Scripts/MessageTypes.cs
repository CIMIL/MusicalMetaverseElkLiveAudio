using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessageTypes { 

    public enum MessageType
    {
        Position,
        SoundData
    }

    public class Message
    {
        public MessageType messageType;
    }

    public class Position : Message
    {
        public Vector3 position;
        public Quaternion rotation;

        public Position()
        {
            messageType = MessageType.Position;
        }
    }
    public class SoundData : Message
    {
        public int index;

        public SoundData()
        {
            messageType = MessageType.SoundData;
        }
    }
}