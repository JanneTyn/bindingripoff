using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    [SerializeField] private AudioMixer audioMixerAsset;
    public AudioClipListAsset audioClipListAsset; //for retention outside prefabs

    public void PlaySFX(AudioClip audio, Vector3 position)
    {
        var gameObject = new GameObject("AudioSourceObject");
        gameObject.transform.position = position;
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audio;
        audioSource.outputAudioMixerGroup = audioMixerAsset.FindMatchingGroups("Master/SFX")[0];
        audioSource.Play();
        Destroy(gameObject, audio.length);
    }
}
