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
    private PlayerMovement _playerMovement;

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

    [Header("Camera")]
    public GameObject MainCameraObject;
    private Camera MainCamera;
    private FollowCamera MainCameraFollow;
    public float CameraZoomSpeed;
    public float MaxZoomOut;
    public float MaxZoomIn;
    public bool IsZoomingIn, IsZoomingOut, IsZoomLevel;
    
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

        _playerMovement = Player.GetComponent<PlayerMovement>();
        Player = _playerMovement.gameObject;
        MainCamera = MainCameraObject.GetComponent<Camera>();
        MainCameraFollow = MainCameraObject.GetComponent<FollowCamera>();
        
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
        if (IsZoomingIn) ZoomInCamera();
        else if (IsZoomingOut) ZoomOutCamera();
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
        if (_playerMovement.isInvincible) return;
        _playerMovement.invincibilityTimer = _playerMovement.invincibilityDuration;
        _playerMovement.isInvincible = true;
        IncreaseDeathCount();
        ResetSpawn();
        UpdateDeathCountDisplay();
    }

    public void ToggleFollowCamera()
    {
        MainCamera.GetComponent<FollowCamera>().IsActive = !MainCamera.GetComponent<FollowCamera>().IsActive;
    }

    public void ZoomOutCamera()
    {
        MainCamera.orthographicSize += Time.deltaTime * CameraZoomSpeed;
        if (MainCamera.orthographicSize <= MaxZoomIn)
        {
            IsZoomingIn = false;
        }
    }

    public void ZoomInCamera()
    {
        MainCamera.orthographicSize -= Time.deltaTime * CameraZoomSpeed;
        if (MainCamera.orthographicSize >= MaxZoomOut)
        {
            IsZoomingOut = false;
        }
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
            if (IsZoomLevel)
            {
                IsZoomingIn = true;
                MainCameraFollow.IsActive = true;
            }
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
