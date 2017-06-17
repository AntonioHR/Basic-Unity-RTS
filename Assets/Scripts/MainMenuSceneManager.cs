using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour {
    [System.Serializable]
    public class Settings
    {
        public string gameSceneName;
    }

    public Settings settings;
    
    public void StartBattle()
    {
        SceneManager.LoadScene(settings.gameSceneName);
    }

}
