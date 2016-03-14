﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public static MainMenuManager Instance { get; private set; }
    public Transform mainMenu, oldMenu;

    void Start()
    {
        oldMenu = mainMenu;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ArenaScene");
    }

    public void GenericMenuChange(Transform newMenu)
    {
        newMenu.gameObject.SetActive(true);
        oldMenu.gameObject.SetActive(false);
        oldMenu = newMenu;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
