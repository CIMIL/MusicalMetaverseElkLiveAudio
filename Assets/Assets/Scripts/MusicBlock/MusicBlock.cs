using UnityEngine;

public class MusicBlock : MonoBehaviour
{
    public SoundList soundList;
    public UnityEngine.Color idleColor;
    public UnityEngine.Color pressedColor;

    public string interactableTag;
    public int soundIndex;
    public bool activateLogging;

    private void Start()
    {
        GetComponent<Renderer>().material.color = idleColor;
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
        {
            GetComponent<AudioSource>().PlayOneShot(soundList.sounds[soundIndex]);
            GetComponent<Renderer>().material.color = pressedColor;
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.transform.CompareTag(interactableTag))
            GetComponent<Renderer>().material.color = idleColor;
    }

    public void SetSoundIndex(int index) {
        soundIndex = index;
    }
}
