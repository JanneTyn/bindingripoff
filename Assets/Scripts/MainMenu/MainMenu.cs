using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public GameObject SoundMenu;
    public GameObject MainMainMenu;
    public GameObject Controls;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    void Start()
    {
        InitialiseResolutions();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void InitialiseResolutions()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResolutionIndex;

        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void OpenSoundMenu()
    {
        SoundMenu.SetActive(!SoundMenu.activeSelf);
        MainMainMenu.SetActive(!MainMainMenu.activeSelf);
    }

    public void OpenControlsMenu()
    {
        MainMainMenu.SetActive(!MainMainMenu.activeSelf);
    }

}
