using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoundList")]
public class SoundList : ScriptableObject
{
    public List<AudioClip> sounds;

    public int IndexOf(AudioClip sound)
    {
        return sounds.IndexOf(sound);
    }
}
