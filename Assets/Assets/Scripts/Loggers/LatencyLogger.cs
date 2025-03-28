using System.Collections;
using System.Collections.Generic;
using Ubiq.Logging;
using Ubiq.Messaging;
using UnityEngine;
using EventType = Ubiq.Logging.EventType;

public class LatencyLogger : MonoBehaviour
{
    private LogEmitter logEmitter;
    private LatencyMeter latencyMeter;

    private void Start()
    {
        logEmitter = new InfoLogEmitter(this);
        latencyMeter = GetComponent<LatencyMeter>();
    }

    private void Update()
    {
        latencyMeter.MeasurePeerLatencies();
    }

    public void WriteLog(LatencyMeter.Measurement measurement)
    {
        logEmitter.Log("Latency Measurement", new LogData(
            measurement.source, 
            measurement.destination,
            measurement.time,
            measurement.frameTime)
        );
    }

    private struct LogData
    {
        public string Source;
        public string Destination;
        public float Time;
        public float FrameTime;

        public LogData(NetworkId source, NetworkId destination, float time, float frameTime)
        {
            Source = source.ToString();
            Destination = destination.ToString();
            Time = time;
            FrameTime = frameTime;
        }
    }
}

public class ApplicationLogEmitter : LogEmitter
{
    public ApplicationLogEmitter(MonoBehaviour component) : base(EventType.Application, component)
    {
    }
}



