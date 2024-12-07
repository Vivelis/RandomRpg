using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class MainMenuController : MonoBehaviour
{
    [Header("Levels To load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Exit")]
    [SerializeField] private GameObject ConfirmationPrompt = null;

    [Header("Settings Panel")]
    [SerializeField] private GameObject GameTitle = null;
    [SerializeField] private GameObject SettingsPanel = null;

    [Header("Volume Settings")]
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeValueText = null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Gameplay Settings")]
    [SerializeField] private Slider controllerSensitivitySlider = null;
    [SerializeField] private TMP_Text controllerSensitivityValueText = null;
    [SerializeField] private float defaultControllerSensitivity = 50;
    public int mainControllerSensitivity = 50;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private Toggle fullscreenToggle = null;
    private Resolution[] resolutions;
    private int _resolutionIndex;
    private int _qualityLevel;
    private bool _isFullScreen;

    private DataPersistenceManager _dataPersistenceManager;

    private void Awake() {
        _dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
        Assert.IsNotNull(_dataPersistenceManager, "DataPersistenceManager not found in scene");
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            currentResolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void OnNewGameButtonPressed()
    {
        _dataPersistenceManager.enabled = true;
        _dataPersistenceManager.NewGame();
        SceneManager.LoadScene(_newGameLevel);
    }

    public void OnLoadGameButtonPressed()
    {
        _dataPersistenceManager.enabled = true;
        _dataPersistenceManager.LoadGame();
        if (_dataPersistenceManager.HasGameData())
        {
            levelToLoad = _dataPersistenceManager.GetGameData().currentScene;
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

    public void OnSettingsButtonPressed()
    {
        if (SettingsPanel.activeSelf)
        {
            SettingsPanel.SetActive(false);
            GameTitle.SetActive(true);
        }
        else
        {
            SettingsPanel.SetActive(true);
            GameTitle.SetActive(false);
        }
    }

    public void OnVolumeSliderValueChanged(float volume)
    {
        AudioListener.volume = volume;
        volumeValueText.text = (volume * 100).ToString("0");
    }

    public void OnApplyVolumeButtonPressed()
    {
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void OnControllerSensitivitySliderValueChanged(float sensitivity)
    {
        mainControllerSensitivity= Mathf.RoundToInt(sensitivity);
        controllerSensitivityValueText.text = mainControllerSensitivity.ToString("0");
    }

    public void OnApplyControllerSensitivityButtonPressed()
    {
        ApplyControllerSensitivity();
    }

    public void ApplyControllerSensitivity()
    {
        PlayerPrefs.SetInt("controllerSensitivity", mainControllerSensitivity);
        StartCoroutine(ConfirmationBox());
    }

    public void OnResetControllerSensitivityButtonPressed()
    {
        controllerSensitivitySlider.value = defaultControllerSensitivity;
        mainControllerSensitivity = Mathf.RoundToInt(defaultControllerSensitivity);
        controllerSensitivityValueText.text = mainControllerSensitivity.ToString("0");
        ApplyControllerSensitivity();
    }

    public void OnResetVolumeButtonPressed()
    {
        volumeSlider.value = defaultVolume;
        AudioListener.volume = defaultVolume;
        volumeValueText.text = (defaultVolume * 100).ToString("0");
        ApplyVolume();
    }

    public void OnResolutionDropdownValueChanged(int resolutionIndex)
    {
        _resolutionIndex = resolutionIndex;
    }

    public void OnQualityDropdownValueChanged(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void OnFullscreenToggleValueChanged(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    public void OnApplyGraphicsButtonPressed()
    {
        ApplyGraphics();
    }

    public void ApplyGraphics()
    {
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("qualityLevel", _qualityLevel);
        Screen.fullScreen = _isFullScreen;
        PlayerPrefs.SetInt("fullscreen", _isFullScreen ? 1 : 0);
        Resolution resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _isFullScreen);
        PlayerPrefs.SetInt("resolutionIndex", _resolutionIndex);
        StartCoroutine(ConfirmationBox());
    }

    public void OnResetGraphicsButtonPressed()
    {
        resolutionDropdown.value = resolutions.Length - 1;
        qualityDropdown.value = 2;
        fullscreenToggle.isOn = true;
        ApplyGraphics();
    }

    public IEnumerator ConfirmationBox()
    {
        ConfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        ConfirmationPrompt.SetActive(false);
    }
}
