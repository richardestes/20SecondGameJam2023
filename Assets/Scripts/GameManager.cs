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
    
    public GameState currentState, previousState;

    [Header("Stopwatch")]
    public float TimeLimit;
    public TMP_Text StopwatchDisplay;
    
    [Header("Screens")]
    public GameObject PauseScreen;
    public GameObject GameOverScreen;
    
    public bool IsGameOver = false;

    private void Awake()
    {
        // Singleton and ready to mingleton
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("ERROR: Extra " + this + " deleted");
            Destroy(gameObject);
        }
        DisableScreens();
        ResetStopwatchColor();
    }
    
    private void Update()
    {
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
                // INSERT NEXT LEVEL LOGIC HERE
                break;
            default:
                Debug.LogError("ERROR: State does not exist");
                break;
        }
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
    
    void UpdateStopwatch()
    {
        TimeLimit -= Time.deltaTime;
        if (TimeLimit <= 10f)
        {
            ChangeStopwatchColor();
        }
        if (TimeLimit <= 0f)
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
        int seconds = Mathf.FloorToInt(TimeLimit % 60);
        StopwatchDisplay.text = seconds.ToString();
    }
}
