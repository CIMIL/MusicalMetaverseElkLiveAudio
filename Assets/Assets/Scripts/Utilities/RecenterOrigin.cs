using System;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Logging;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RecenterOrigin : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    private XROrigin origin;
    private LogEmitter debugLogEmitter;

    private void Start()
    {
        origin = GetComponent<XROrigin>();
        debugLogEmitter = new ExperimentLogEmitter(this);
        StartCoroutine(Recenter());
    }

    private IEnumerator Recenter()
    {
        int times = 0;
        while (times < 2)
        {
            origin.MoveCameraToWorldLocation(spawnPosition.position);
            origin.MatchOriginUpCameraForward(spawnPosition.up, spawnPosition.forward);
            times++;
            yield return null;
        }
    }
}
