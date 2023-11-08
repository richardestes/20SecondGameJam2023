using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{           
    public static GameManager instance;
    
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        NextLevel
    }
    public GameObject Player;
    public GameObject PlayerSpawn;

    [Header("GameState")]
    public GameState currentState, previousState;

    [Header("Stats")]
    public int Score;
    public int DeathCount;
    public TMP_Text DeathCountDisplay;
    
    [Header("Audio")]
    public AudioClip MainAudioClip;
    public AudioSource MainAudioSource;
    
    [Header("Stopwatch")]
    public float TimeLimit;
    private float CurrentTime;
    public TMP_Text StopwatchDisplay;
    
    [Header("Screens")]
    public GameObject PauseScreen;
    public GameObject GameOverScreen;
    
    public bool IsGameOver, IsChangingLevel = false;

    private void Awake()
    {
        // Singleton Section
        if (instance != null)
        {
            Debug.LogError("ERROR: Extra " + this + " deleted");
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);

        Player = FindObjectOfType<PlayerMovement>().gameObject;
        
        DisableScreens();
        ResetStopwatch();
        ResetStopwatchColor();
        UpdateDeathCountDisplay();
        
        // DEBUG: Printing Stats
        Debug.LogWarning("Death Count: " + DeathCount);
        Debug.LogWarning("Score: "+ Score);
        
        // Audio shit
        MainAudioSource.clip = MainAudioClip;
        MainAudioSource.Play();
        MainAudioSource.volume = .2f;
    }

    private void Update()
    {
        if (SceneController.GetCurrentSceneIndex() == 0)
        {
            StopAudio();
        }
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                // We check this in order to prevent DisplayResults() from being executed multiple times, since
                // this is occuring in Update()
                if (!IsGameOver) 
                {
                    IsGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("Game is over");
                    DisplayGameOverScreen();
                }
                break;
            case GameState.NextLevel:
                if (!IsChangingLevel)
                {
                    IsChangingLevel = true;
                    Debug.Log("Level Completed!");
                    int sceneIndex = SceneController.GetCurrentSceneIndex();
                    sceneIndex++;
                    SceneController.ChangeSceneByIndex(sceneIndex);
                }
                break;
            default:
                Debug.LogError("ERROR: State does not exist");
                break;
        }
  
    }

    public void Die()
    {
        IncreaseDeathCount();
        ResetSpawn();
        UpdateDeathCountDisplay();
    }

    public void UpdateDeathCountDisplay()
    {
        DeathCountDisplay.text = DeathCount.ToString();
    }

    void ResetSpawn()
    {
        Player.transform.position = PlayerSpawn.transform.position;
    }

    void IncreaseDeathCount()
    {
        DeathCount++;
    }

    public void IncreaseScore(int amount)
    {
        Score += amount;
    }

    public void StopAudio()
    {
        MainAudioSource.Stop();
    }
    
    void DisableScreens()
    {
        PauseScreen.SetActive(false);
        GameOverScreen.SetActive(false);
    }

    void DisplayGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    
    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            PauseScreen.SetActive(true);
            Debug.Log("Game is Paused");
        }
    }
    
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState); // ensure that the game continues from the state it was paused from
            Time.timeScale = 1f;
            PauseScreen.SetActive(false);
            Debug.Log("Game is Resumed");
        }
    }
    
    public void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused) ResumeGame();
            else PauseGame();
        }
    }
    
    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void ResetStopwatch()
    {
        CurrentTime = TimeLimit;
    }
    
    void UpdateStopwatch()
    {
        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 10f)
        {
            ChangeStopwatchColor();
        }
        if (CurrentTime <= 0f)
        {
            ChangeState(GameState.GameOver);
            return;
        }
        UpdateStopwatchDisplay();
    }

    void ChangeStopwatchColor()
    {
        StopwatchDisplay.color = Color.red;
    }

    void ResetStopwatchColor()
    {
        StopwatchDisplay.color = Color.white;
    }
    
    void UpdateStopwatchDisplay()
    {
        int seconds = Mathf.FloorToInt(CurrentTime % 60);
        StopwatchDisplay.text = seconds.ToString();
    }
}
