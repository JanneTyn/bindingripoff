using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Multiple instances of AudioManager");
            enabled = false;
        }
        else instance = this;

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] public AudioMixer audioMixerAsset;
    public AudioClipListAsset audioClipListAsset; //for retention outside prefabs

    private void Start()
    {
        if (AudioUpdate.instance != null) { AudioUpdate.instance.UpdateAudio(); } 
    }
    public void PlaySFX(AudioClip audio, Vector3 position)
    {
        var obj = new GameObject("AudioSourceObject");
        obj.transform.position = position;
        var audioSource = obj.AddComponent<AudioSource>();
        audioSource.clip = audio;
        audioSource.outputAudioMixerGroup = audioMixerAsset.FindMatchingGroups("Master/SFX")[0];
        audioSource.Play();
        Destroy(obj, audio.length);
    }

    public void PlayUISFX()
    {
        var obj = new GameObject("AudioSourceObject");
        var audioSource = obj.AddComponent<AudioSource>();
        audioSource.clip = audioClipListAsset.uiButtonPress;
        audioSource.outputAudioMixerGroup = audioMixerAsset.FindMatchingGroups("Master/SFX")[0];
        audioSource.Play();
        Destroy(obj, audioClipListAsset.uiButtonPress.length);
    }
}
