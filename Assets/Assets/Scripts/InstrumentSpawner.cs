using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Spawning;
using UnityEngine;

public class InstrumentSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public NetworkScene scene;

    private GameObject userInstrument;
    protected NetworkSpawnManager manager;

    void Start()
    {
        manager = scene.GetComponentInChildren<NetworkSpawnManager>();
        manager.OnSpawned.AddListener(OnSpawned);
    }

    private void OnSpawned(GameObject go, IRoom room, IPeer peer, NetworkSpawnOrigin origin)
    {
        if (go.CompareTag("Instrument") && origin == NetworkSpawnOrigin.Remote)
        {
            SyncedTransform st = go.GetComponent<SyncedTransform>();
            st.Start();
            st.SyncLastPosition();

            go.GetComponent<OSCSendOnTrigger>().enabled = false;
        }
    }
    public void Spawn(int index)
    {
        if (!manager)
            return;

        // TODO: Fix a bug where once you respawn all instruments at least once, their position is merged together
        if (userInstrument)
            manager.Despawn(userInstrument);

        userInstrument = manager.SpawnWithPeerScope(manager.catalogue.prefabs[index]);
        spawnPoint.GetPositionAndRotation(out var pos, out var rot);
        userInstrument.transform.SetPositionAndRotation(pos, rot);
    }
}
