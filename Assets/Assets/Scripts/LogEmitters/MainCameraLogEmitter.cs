using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TinyJson;
using Ubiq.Logging;
using Ubiq.Messaging;
using UnityEngine;

public class MainCameraLogEmitter : MonoBehaviour
{
    private LogEmitter logEmitter;

    private void Start()
    {
        logEmitter = new InfoLogEmitter(this);
        
        InvokeRepeating(nameof(_logVector), 0.0f, 0.3f);
    }

    private void _logVector()
    {
        logEmitter.Log("Camera direction and position vectors", transform.forward, transform.position );
    }
}
