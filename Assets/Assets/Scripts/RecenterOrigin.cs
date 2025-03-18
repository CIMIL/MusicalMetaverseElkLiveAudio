using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RecenterOrigin : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    bool once = true;
    private XROrigin origin;
    
    private void Start()
    {
        origin = GetComponent<XROrigin>();
    }

    private void Update()
    {
        if (!once)
            return;
        
        origin.MoveCameraToWorldLocation(spawnPosition.position);
        origin.MatchOriginUpCameraForward(spawnPosition.up, spawnPosition.forward);
        
        if (origin.Camera.transform.position == spawnPosition.position)
        {
            once = false;
        }

    }
}
