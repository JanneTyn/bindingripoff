using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    public InputActionAsset inputActions;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    void Start()
    {
        InitialiseResolutions();

      /**  inputActions = GetComponent<PlayerInput>().actions;
        InputAction dodgeAction = inputActions.FindAction("Gameplay/Dodge");
        RebindKey(dodgeAction, "X");**/
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

    public void RebindKey(InputAction action, string newKey)
    {
        if (rebindingOperation != null)
        {
            rebindingOperation.Dispose();
        }

        rebindingOperation = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithBindingGroup(newKey)
            .OnComplete(operation => {
                rebindingOperation.Dispose();
            })
            .Start();
    }
}


