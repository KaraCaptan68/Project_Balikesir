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
        Cursor.lockState = CursorLockMode.Locked; // Farenin kilitli oldu�undan emin olun
        Cursor.visible = false; // Fareyi g�r�nmez yap�n
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
        //Debug.Log("Is game paused = " + �sGamePaused);
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Oyun devam ederken fareyi kilitli yap�n
        Cursor.visible = false; // Fareyi g�r�nmez yap�n
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None; // Oyun durdu�unda fareyi serbest b�rak�n
        Cursor.visible = true; // Fareyi g�r�n�r yap�n
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
