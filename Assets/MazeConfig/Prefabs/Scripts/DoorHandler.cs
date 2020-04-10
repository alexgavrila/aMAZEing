
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Small utility used to open and close door
[RequireComponent(typeof(Animator))]
public class DoorHandler : MonoBehaviour
{
    private Animator anim;

    private const string OpenDoorTrig = "DoorOpening";
    private const string CloseDoorTrig = "DoorClosing";

    private bool isDoorClosed = true;
    
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OpenDoor()
    {
        anim.SetTrigger(OpenDoorTrig);

        isDoorClosed = false;
    }

    private void CloseDoor()
    {
        anim.SetTrigger(CloseDoorTrig);

        isDoorClosed = true;
    }

    public void InteractWithDoor()
    {
        if (isDoorClosed)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
}
