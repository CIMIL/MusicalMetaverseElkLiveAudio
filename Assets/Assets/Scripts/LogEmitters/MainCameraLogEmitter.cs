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
    private MusicBlock _block;
    private LogEmitter _logEmitter;

    private void Start()
    {
        _block = GetComponent<MusicBlock>();
        _logEmitter = new InfoLogEmitter(this);
        
        InvokeRepeating(nameof(_logVector), 0.0f, 0.3f);
    }

    private void _logVector()
    {
        _logEmitter.Log("Head vector", this.transform.forward.ToString());
    }
}
