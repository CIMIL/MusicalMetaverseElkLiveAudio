using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OSCSendOnTrigger : MonoBehaviour
{
    private MusicBlock block;
    private OSCBlockGroup group;

    private void Start()
    {
        block = GetComponent<MusicBlock>();
        group = GetComponentInParent<OSCBlockGroup>();
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(block.interactableTag))
        {
            group.Send(block.soundIndex, true);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(block.interactableTag))
        {
            group.Send(block.soundIndex, false);
        }
    }
}
