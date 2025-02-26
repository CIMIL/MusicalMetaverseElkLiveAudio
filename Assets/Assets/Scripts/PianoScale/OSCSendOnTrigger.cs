using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OSCSendOnTrigger : MonoBehaviour
{
    [SerializeField]
    private InstrumentBlock block;
    private BlockGroup group;

    private void Start()
    {
        block = GetComponent<InstrumentBlock>();
        group = GetComponentInParent<BlockGroup>();
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(block.interactableTag))
        {
            group.Send(true, block.blockGroup.octave * 12 + block.sequenceNumber);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(block.interactableTag))
        {
            group.Send(false, block.blockGroup.octave * 12 + block.sequenceNumber);
        }
    }
}
