using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private DataPersistenceManager dataPersistenceManager;
    private GameObject pauseMenu;

    private void Start() {
        dataPersistenceManager = DataPersistenceManager.instance;
        pauseMenu = transform.GetChild(0).gameObject;
        pauseMenu.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Resume();
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;

        if (dataPersistenceManager != null)
        {
            dataPersistenceManager.SaveGame();
        }
        else {
            Debug.LogWarning("DataPersistenceManager not found in scene");
        }
        SceneManager.LoadScene("MainMenu");
    }
}
