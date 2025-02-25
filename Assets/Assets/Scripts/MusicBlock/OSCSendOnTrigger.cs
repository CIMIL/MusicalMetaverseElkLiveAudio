using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OSCSendOnTrigger : MonoBehaviour
{
    [SerializeField]
    private int note;
    private MusicBlock block;
    private BlockGroup group;

    private void Start()
    {
        block = GetComponent<MusicBlock>();
        group = GetComponentInParent<BlockGroup>();
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(block.interactableTag))
        {
            group.Send(true, note);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(block.interactableTag))
        {
            group.Send(false, note);
        }
    }
}
