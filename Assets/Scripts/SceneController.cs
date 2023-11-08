using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static void ChangeSceneByName(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1; // reset to default value
    }

    public static void ChangeSceneByIndex(int index)
    {
       SceneManager.LoadScene(index);  
       Time.timeScale = 1; // reset to default value
    }

    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
