using System.Collections;
using System.Collections.Generic;
using extOSC;
using Ubiq.Logging;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Spawning;
using UnityEngine;

public class InstrumentSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public NetworkScene scene;

    [SerializeField] private bool logging;

    private LogEmitter logEmitter;
    private GameObject userInstrument;
    private NetworkSpawnManager manager;

    void Start()
    {
        manager = scene.GetComponentInChildren<NetworkSpawnManager>();
        manager.OnSpawned.AddListener(OnSpawned);

        if (logging)
            logEmitter = new ExperimentLogEmitter(this);
    }

    private void OnSpawned(GameObject go, IRoom room, IPeer peer, NetworkSpawnOrigin origin)
    {
        if (go.CompareTag("Instrument") && origin == NetworkSpawnOrigin.Local)
        {
            userInstrument = go;
            
            SyncedTransform st = go.GetComponent<SyncedTransform>();
            st.Start();
            spawnPoint.GetPositionAndRotation(out var pos, out var rot);
            st.transform.SetPositionAndRotation(pos, rot);
        }
        else if (go.CompareTag("Instrument") && origin == NetworkSpawnOrigin.Remote)
        {
            go.GetComponent<BlockGroup>().Disable();
            
            SyncedTransform st = go.GetComponent<SyncedTransform>();
            st.Start();
            st.SyncLastPosition();
        }
    }
    public void Spawn(int index)
    {
        if (manager)
        {
            if (userInstrument)
            {
                SyncedTransform st = userInstrument.GetComponent<SyncedTransform>();
                spawnPoint.GetPositionAndRotation(out var pos, out var rot);
                st.transform.SetPositionAndRotation(pos, rot);
            }
            else
            {
                manager.SpawnWithRoomScope(manager.catalogue.prefabs[index]);
                if (logging)
                    logEmitter.Log("Piano Spawned");
            }
        }
    }
}
