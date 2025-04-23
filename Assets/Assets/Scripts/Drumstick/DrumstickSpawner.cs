using System.Collections;
using System.Collections.Generic;
using Ubiq.Logging;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Spawning;
using UnityEngine;

public class DrumstickSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public NetworkScene scene;

    [SerializeField] private bool logging;
    
    private LogEmitter logEmitter;
    private NetworkSpawnManager manager;

    void Start()
    {
        manager = scene.GetComponentInChildren<NetworkSpawnManager>();
        manager.OnSpawned.AddListener(OnSpawned);

        if (logging)
        {
            logEmitter = new ExperimentLogEmitter(this);
        }
    }
    
    private void OnSpawned(GameObject go, IRoom room, IPeer peer, NetworkSpawnOrigin origin)
    {
        if (go.CompareTag("Drumstick") && origin == NetworkSpawnOrigin.Local)
        {
            
            SyncedTransform st = go.GetComponent<SyncedTransform>();
            st.Start();
            spawnPoint.GetPositionAndRotation(out var pos, out var rot);
            st.transform.SetPositionAndRotation(pos, rot);
            
            Drumstick d = go.GetComponent<Drumstick>();
            d.Start();
            d.grabbedBy = NetworkScene.Find(this).Id;
            d.SendUpdateMessage();
        } else if (go.CompareTag("Drumstick") && origin == NetworkSpawnOrigin.Remote)
        {
            SyncedTransform st = go.GetComponent<SyncedTransform>();
            st.Start();
            st.SyncLastPosition();
        }
    }

    public void Spawn(int index)
    {
        if (!manager) return;
        manager.SpawnWithRoomScope(manager.catalogue.prefabs[index]);

        if (logging)
        {
            logEmitter.Log("Drumstick Spawned");
        }
    }
}
