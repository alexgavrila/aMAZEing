using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

[RequireComponent(typeof(CombatTarget))]
public class InGameUIController : MonoBehaviour
{
    public Canvas inGameUICanvas;

    public GameObject gameOverPanel;
    public GameObject inGamePanel; 
    
    // Display the health, coins and enemies killed
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI enemiesKilledText;
    // Flash a message on the screen then it slowly fades out and disappears
    public TextMeshProUGUI flashMessage;

    public string healthTextAppend = "Health: ";
    public string coinsTextAppend = "Picked Coins: ";
    public string enemiesTextAppend = "Enemies Killed: ";

    public float flashMessageTime = 1f;
    
    // Display the health stats
    private CombatTarget stats;
    private Player player;

    private Coroutine fadeOutCoroutine;

    public void OnGameOverPanel()
    {
        GameManager.instance.isGameOver = true;
            
        gameOverPanel.SetActive(true);
        inGamePanel.SetActive(false);
            
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnGameRestart()
    {
        GameManager.instance.isGameOver = false;
        
        gameOverPanel.SetActive(false);
        inGamePanel.SetActive(true);

        GameManager.instance.enemyManager.DespawnEnemies();
        
        GameManager.instance.RestartGame();
        GameManager.instance.PlayerInstance.Restore();
        
        Time.timeScale = 1f;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void FlashMessage(string message)
    {
        if (GameMenuUI.IsGamePaused)
        {
            return;
        }

        flashMessage.text = message;
        SetFadeMessageAlpha(1);

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        
        fadeOutCoroutine = StartCoroutine(FadeMessage());
    }

    private IEnumerator FadeMessage()
    {
        for (float alpha = 1; alpha >= 0; alpha -= 0.25f)
        {
            SetFadeMessageAlpha(alpha);

            yield return new WaitForSeconds(flashMessageTime / 4);
        }
    }

    private void SetFadeMessageAlpha(float alpha)
    {
        var color = flashMessage.color;
        color.a = alpha;
        
        flashMessage.color = color;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        stats = GetComponent<CombatTarget>();
        player = GetComponent<Player>();
        
        SetFadeMessageAlpha(0);
        
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // When the game is being paused, no in game ui canvas is showing
        if (GameMenuUI.IsGamePaused)
        {
            inGameUICanvas.gameObject.SetActive(false);
            return;
        }
        
        inGameUICanvas.gameObject.SetActive(true);
        UpdateTextUi();
    }

    private void UpdateTextUi()
    {
        healthText.text = healthTextAppend + stats.currHealth + " / " + stats.maxHealth;
        coinsText.text = coinsTextAppend + player.PickedCoins;
        enemiesKilledText.text = enemiesTextAppend + player.EnemiesKilled + " / " + LevelParams.enemiesToKill;
    }
}
