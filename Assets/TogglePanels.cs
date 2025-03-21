using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TogglePanels : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private GameObject visualTarget;

    public bool panelsEnabled = true;

    public void Toggle()
    {
        panelsEnabled = !panelsEnabled;
        
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panelsEnabled);
        }

        visualTarget.GetComponent<MeshRenderer>().material = panelsEnabled ? enabledMaterial : disabledMaterial;
    }
    
}
