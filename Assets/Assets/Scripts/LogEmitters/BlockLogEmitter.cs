using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Ubiq.Logging;
using Ubiq.Messaging;
using Ubiq.Rooms;
using UnityEngine;

public class BlockLogEmitter : MonoBehaviour
{
    private MusicBlock block;
    private LogEmitter interactions;

    private void Start()
    {
        block = GetComponent<MusicBlock>();
        interactions = new ExperimentLogEmitter(this);
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        /* The purpose of this script is to properly log which user is locally interacting with a block and which
         users are receiving the interaction through the network. The script does this by checking which peer is
         holding the drumstick. There's an edge case to be found here: a block can be triggered by a drumstick even
         if nobody is holding it, by moving the block itself. */

        if (!other.gameObject.transform.CompareTag(block.interactableTag)) return;
        
        // local tells me if the drumstick that is playing a block is grabbed by you (local) or by someone else (remote)
        bool local = other.gameObject.GetComponentInParent<Drumstick>().grabbedBy == NetworkScene.Find(this).Id;
        EventData e = new EventData("Entered", local ? "Local" : "Remote");
        interactions.Log("Block Interaction", e);
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (!other.gameObject.transform.CompareTag(block.interactableTag)) return;

        bool local = other.gameObject.GetComponentInParent<Drumstick>().grabbedBy == NetworkScene.Find(this).Id;
        EventData e = new EventData("Exited", local ? "Local" : "Remote");
        interactions.Log("Block Interaction", e);
    }
    private struct EventData
    {
        public string type;
        public string network;

        public EventData(string type, string network)
        {
            this.type = type;
            this.network = network;
        }
    }

}