using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

internal class UnityMainThread : MonoBehaviour
{
    internal static UnityMainThread Wkr;
    private Queue<Action> jobs = new Queue<Action>();

    void Awake()
    {
        Wkr = this;
    }

    void Update()
    {
        while (jobs.Count > 0)
            jobs.Dequeue().Invoke();
    }

    internal void AddJob(Action newJob)
    {
        jobs.Enqueue(newJob);
    }
}
