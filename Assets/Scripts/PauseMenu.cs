using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;

    private void Start()
    {
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Farenin kilitli olduðundan emin olun
        Cursor.visible = false; // Fareyi görünmez yapýn
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        //Debug.Log("Is game paused = " + ýsGamePaused);
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Oyun devam ederken fareyi kilitli yapýn
        Cursor.visible = false; // Fareyi görünmez yapýn
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None; // Oyun durduðunda fareyi serbest býrakýn
        Cursor.visible = true; // Fareyi görünür yapýn
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
