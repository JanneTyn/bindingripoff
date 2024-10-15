using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioUpdate : MonoBehaviour
{
    public static AudioUpdate instance;

    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;

    private float masterVolume = -20f;
    private float musicVolume = -20f;
    private float sfxVolume = -20f;

    public const string master = "MasterVolume";
    public const string music = "MusicVolume";
    public const string sfx = "SFXvolume";

    private float minVolume = -80f;
    private float maxVolume = 20f;


    void Start()
    {
        masterSlider = GameObject.Find("Master").GetComponent<Slider>();
        musicSlider = GameObject.Find("Music").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFX").GetComponent<Slider>();

        masterSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        musicSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        sfxSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        GameObject.Find("sound").SetActive(false);
    }

    public void ValueChangeCheck()
    {
        masterVolume = Mathf.Lerp(minVolume, maxVolume, masterSlider.value);
        musicVolume = Mathf.Lerp(minVolume, maxVolume, musicSlider.value);
        sfxVolume = Mathf.Lerp(minVolume, maxVolume, sfxSlider.value);
    }

    public void UpdateAudio()
    {
        AudioManager.instance.audioMixerAsset.SetFloat(master, masterVolume);
        AudioManager.instance.audioMixerAsset.SetFloat(music, musicVolume);
        AudioManager.instance.audioMixerAsset.SetFloat(sfx, sfxVolume);
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
