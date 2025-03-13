using System.Collections;
using System.Collections.Generic;
using Ubiq.Logging;
using Ubiq.Messaging;
using UnityEngine;
using EventType = Ubiq.Logging.EventType;

public class LatencyLogger : MonoBehaviour
{
    private LogEmitter logEmitter;
    

    private void Start()
    {
        logEmitter = new ApplicationLogEmitter(this);
    }

    public void WriteLog(LatencyMeter.Measurement measurement)
    {
        logEmitter.Log("Latency Measurement", new LogData(
            measurement.source, 
            measurement.destination,
            measurement.time,
            measurement.frameTime)
        );
        Debug.Log(measurement.source + " - " + measurement.destination);
    }

    private struct LogData
    {
        public NetworkId Source;
        public NetworkId Destination;
        public float Time;
        public float FrameTime;

        public LogData(NetworkId source, NetworkId destination, float time, float frameTime)
        {
            Source = source;
            Destination = destination;
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



