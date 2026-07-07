using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button exitButton;
    public static bool HasSave { get; private set; }
    private void Start()
    {
        newGameButton.onClick.AddListener(OnNewGame);
        loadGameButton.onClick.AddListener(OnLoadGame);
        exitButton.onClick.AddListener(OnExit);
        
        string savePath = Application.persistentDataPath + "/save.json";
        loadGameButton.interactable = File.Exists(savePath);
    }

    private void OnNewGame()
    {
        HasSave = false;
        SceneManager.LoadScene("SampleScene");
    }
    private void OnLoadGame()
    {
        HasSave = true;
        SceneManager.LoadScene("SampleScene");
    }
    private void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
