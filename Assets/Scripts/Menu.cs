using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Menu : MonoBehaviour {

    public void PlayGame() {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }
}
