using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MainMenuController mainMenuController = null;

    [Header("Volume Settings")]
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeValueText = null;

    [Header("Gameplay Settings")]
    [SerializeField] private Slider controllerSensitivitySlider = null;
    [SerializeField] private TMP_Text controllerSensitivityValueText = null;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private Toggle fullscreenToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            LoadSettings();
        }
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float volume = PlayerPrefs.GetFloat("masterVolume");

            volumeSlider.value = volume;
            volumeValueText.text = volume.ToString("0.00");
            AudioListener.volume = volume;
        }
        if (PlayerPrefs.HasKey("controllerSensitivity"))
        {
            float controllerSensitivity = PlayerPrefs.GetFloat("controllerSensitivity");

            controllerSensitivitySlider.value = controllerSensitivity;
            controllerSensitivityValueText.text = controllerSensitivity.ToString("0.00");
            mainMenuController.mainControllerSensitivity = (int)controllerSensitivity;
        }
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("resolutionIndex");

            resolutionDropdown.value = resolutionIndex;
            resolutionDropdown.RefreshShownValue();
            if (resolutionIndex < Screen.resolutions.Length)
            {
                Resolution resolution = Screen.resolutions[resolutionIndex];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            }
        }
        if (PlayerPrefs.HasKey("qualityLevel"))
        {
            int qualityLevel = PlayerPrefs.GetInt("qualityLevel");

            qualityDropdown.value = qualityLevel;
            qualityDropdown.RefreshShownValue();
            QualitySettings.SetQualityLevel(qualityLevel);
        }
        if (PlayerPrefs.HasKey("isFullScreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("isFullScreen") == 1 ? true : false;

            fullscreenToggle.isOn = isFullScreen;
            Screen.fullScreen = isFullScreen;
        }
    }
}
