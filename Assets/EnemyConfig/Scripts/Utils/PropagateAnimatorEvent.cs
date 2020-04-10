using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropagateAnimatorEvent : MonoBehaviour
{
    public AnimatorNavigation navigator; 
    // Just propagate the animator move event to the AnimatorNavigation component on the Enemy Container
    private void OnAnimatorMove()
    {
        navigator.OnAnimatorMove();
    }
}
