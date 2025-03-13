using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Ubiq.Logging;
using Ubiq.Messaging;
using Ubiq.Rooms;
using UnityEngine;

public class BlockInteractionLogger : MonoBehaviour
{
    [SerializeField] private PresetSelector presetSelector;
    private InstrumentBlock block;
    private LogEmitter interactions;
    

    private void Start()
    {
        block = GetComponent<InstrumentBlock>();
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
        var local = other.gameObject.GetComponentInParent<Drumstick>().grabbedBy == NetworkScene.Find(this).Id;
        var e = new EventData("Entered",
            block.note,
            presetSelector.GetActivePreset());
        
        interactions?.Log("Block Interaction", e);
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (!other.gameObject.transform.CompareTag(block.interactableTag)) return;

        var local = other.gameObject.GetComponentInParent<Drumstick>().grabbedBy == NetworkScene.Find(this).Id;
        var e = new EventData("Exited", 
            block.note,
            presetSelector.GetActivePreset());
        
        interactions?.Log("Block Interaction", e);
    }
    private struct EventData
    {
        public string Type;
        public int Note;
        public string Preset;

        public EventData(string type, int note, string preset)
        {
            this.Type = type;
            this.Note = note;
            this.Preset = preset;
        }
    }

}