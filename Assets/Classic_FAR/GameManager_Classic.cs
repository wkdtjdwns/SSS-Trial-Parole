using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Classic : MonoBehaviour
{
    public static GameManager_Classic Instance;

    void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void GameClear()
    {
        Debug.Log("Game Clear");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
