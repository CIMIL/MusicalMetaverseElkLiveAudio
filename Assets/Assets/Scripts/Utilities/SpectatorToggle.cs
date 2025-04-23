using System.Collections;
using System.Collections.Generic;
using Ubiq.Avatars;
using UnityEngine;
using UnityEngine.UIElements;

public class SpectatorToggle : MonoBehaviour
{
    
    [SerializeField] private AvatarManager avatarManager;
    [SerializeField] private GameObject playerPrefab;
    private bool toggled = false;
    
    public void ToggleInvisibility()
    {
        toggled = !toggled;
        avatarManager.avatarPrefab = toggled ? null : playerPrefab;
    }
    
}
