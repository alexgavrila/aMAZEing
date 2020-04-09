using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image HealthBar;
    private CombatTarget combatStats;
    private Canvas healthBarCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        combatStats = GetComponentInParent<CombatTarget>();
        healthBarCanvas = GetComponent<Canvas>();
    }

    // Set the fill of the bar
    void Update()
    {
        UpdateFillAmount();
        UpdateImageRotation();
    }

    private void UpdateImageRotation()
    {
        var currentCamera = GameMenuUI.Instance.CurrentCamera;

        if (currentCamera == null)
        {
            return;
        }

        if (GameMenuUI.IsGamePaused)
        {
            // Rotate the health bar such that it always points to the camera
            healthBarCanvas.transform.rotation = Quaternion.LookRotation(-currentCamera.transform.forward);
        }
        else
        {
            var player = currentCamera.GetComponentInParent<Player>();
            var dirToPlayer = player.transform.position - transform.position;
            
            healthBarCanvas.transform.rotation = Quaternion.LookRotation(dirToPlayer);
        }
    }
    
    void UpdateFillAmount()
    {
        HealthBar.fillAmount = combatStats.currHealth / combatStats.maxHealth;
    }
}
