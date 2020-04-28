using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityScript.Steps;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : MonoBehaviour
{
    public static bool IsGamePaused = true;
    private static bool LastGameState = false;

    #region Singleton
        private static GameMenuUI _instance;
        public static GameMenuUI Instance
        {
            get { return _instance; }
            // Set only once (in the Awake method)
            private set
            {
                if (_instance != null)
                {
                    return;
                }

                _instance = value;
            }
        }
    #endregion

    #region Game Difficulty
        public enum GameDifficulty {
            Easy = 0,
            Medium = 1,
            Hard = 2
        }

        public GameDifficulty gameDifficulty = GameDifficulty.Easy;
    #endregion

    // Get the current camera
    public Camera CurrentCamera
    {
        get { return IsGamePaused ? menuCamera : playerCamera; }
    }

    private Player player;

    // Keep the in game camera so we can deactivate it
    private Camera playerCamera;
    private Camera menuCamera;

    public TextMeshProUGUI highestScore;
    public string highestScoreTextAppend = "HIGHEST SCORE: ";


    public GameMenuUI SetPlayer(Player playerRef)
    {
        var inGameCamera = playerRef.GetComponentInChildren<Camera>();
        playerCamera = inGameCamera;

        player = playerRef;

        return this;
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
    }

    public void RestartGame()
    {
        GameManager.instance.RestartGame();
        IsGamePaused = false;
    }

    public void OnDifficultyChanged(int difficulty)
    {
        gameDifficulty = (GameDifficulty)difficulty;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        menuCamera = GetComponentInChildren<Camera>();
        menuCamera = GetComponentInChildren<Camera>();
        highestScore.text ="pouj;ljk";
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsGamePaused = !IsGamePaused;
        }

        if (LastGameState != IsGamePaused)
        {
            LastGameState = IsGamePaused;

            HandleCameras();
            HandleCursor();

            HandleGamePausedState();
        }

        highestScore.text = highestScoreTextAppend + GameManager.instance.save.name + " / " + GameManager.instance.save.level;

    }

    // Pause the game and hide the player
    private void HandleGamePausedState()
    {
        if (IsGamePaused)
        {
            Time.timeScale = 0f;
            player.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            player.gameObject.SetActive(true);
        }
    }

    private void HandleCameras()
    {
        // If the game is paused, activate the menu camera
        if (IsGamePaused)
        {
            // Deactivate the in game camera
            playerCamera.gameObject.SetActive(false);

            // Activate the menu camera
            menuCamera.gameObject.SetActive(true);
        }
        else
        {
            menuCamera.gameObject.SetActive(false);

            // Activate the in game camera
            playerCamera.gameObject.SetActive(true);
        }
    }


    private void HandleCursor()
    {
        if (IsGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
