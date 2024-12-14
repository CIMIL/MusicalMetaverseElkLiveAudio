using MessageTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Spawning;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public SoundList soundList;
    public TMP_Dropdown soundSelector;
    public NetworkScene scene;

    private NetworkSpawnManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = scene.GetComponentInChildren<NetworkSpawnManager>();
        manager.OnSpawned.AddListener(OnSpawned);
        PopulateDropdown();
    }

    private void PopulateDropdown()
    {
        soundSelector.options.Clear();
        foreach (var sound in soundList.sounds)
        {
            soundSelector.options.Add(new TMP_Dropdown.OptionData(sound.name));
        }
        soundSelector.RefreshShownValue();
    }

    private void OnSpawned(GameObject go, IRoom room, IPeer peer, NetworkSpawnOrigin origin)
    {
        if (go.CompareTag("Music Block") && origin == NetworkSpawnOrigin.Local)
        {
            SyncedTransform st = go.GetComponent<SyncedTransform>();
            st.Start();
            spawnPoint.GetPositionAndRotation(out var pos, out var rot);
            st.transform.SetPositionAndRotation(pos, rot);

            go.GetComponent<MusicBlock>().SetSoundIndex(soundSelector.value);

            SyncSoundIndexOnSpawn ss = go.GetComponent<SyncSoundIndexOnSpawn>();
            ss.Start();
            ss.SyncSound();
        }
    }

    public void Spawn(int index)
    {
        if (manager)
        {
            manager.SpawnWithRoomScope(manager.catalogue.prefabs[index]);
        }
    }
}
