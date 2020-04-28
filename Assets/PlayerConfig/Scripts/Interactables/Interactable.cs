using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string interactionMessage = "Press E to interact";
    public float interactionRange = 1f;

    public UnityEvent onInteraction;

    // keep a reference to the player and his camera
    private Player player;
    private Camera playerCamera;
    // The in-game ui used to display a message to the screen
    // i.e. "Press E to interact"
    private InGameUIController inGameUi;

    // When it was interactable but it isn't anymore invoke event
    private bool isInteractable = false;

    // Update is called once per frame
    private void Update()
    {
        CheckIfInteractable();

        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            onInteraction.Invoke();
        }
    }

    private void CheckIfInteractable()
    {
        player = GameManager.instance.PlayerInstance;

        if (player == null)
        {
            return;
        }

        playerCamera = player.GetComponentInChildren<Camera>();
        inGameUi = player.GetComponent<InGameUIController>();

        if (playerCamera == null || inGameUi == null)
        {
            return;
        }

        var cameraDir = playerCamera.transform.forward;

        if (Vector3.Distance(player.transform.position, transform.position) <= interactionRange)
        {
            // Ignore player layer
            var mask = ~LayerMask.GetMask("Player");

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, cameraDir, out hit, interactionRange, mask))
            {
                // The player is looking at this object
                if (hit.collider.transform == transform)
                {
                    isInteractable = true;

                    inGameUi.FlashMessage(interactionMessage);
                }
                // check if became non interactable
                else
                {
                    isInteractable = false;
                }
            }
        }
        else
        {
            isInteractable = false;
        }
    }
}
