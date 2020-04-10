using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// Code adapted from the Unity Game Development Cookbook chapter 8
[RequireComponent(typeof(NavMeshAgent))]
public class AnimatorNavigation : MonoBehaviour
{
    public UnityEvent onWalking, onIdle;
    
    // The animator of the robot child object
    private Animator anim;
    private NavMeshAgent agent;

    private Vector2 smoothDeltaPos = Vector2.zero;

    private bool movementStopped = false;
    
    #region Constants
    private const string WalkBoolName = "isWalking";
    private const string ForwardSpeedParam = "Speed";
    private const string LateralSpeedParam = "SideSpeed";
    #endregion

    public void SetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    // Deactivate Movement on Death
    public void OnDeath()
    {
        movementStopped = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        // Prevent the agent from changing the character's position
        agent.updatePosition = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        HandleMovment();   
    }

    // If there are any registered events, invoke them
    private void HandleMovementEvents(bool isMoving)
    {
        if (isMoving)
        {
            if (onWalking != null)
            {
                onWalking.Invoke();
            }
        }
        else
        {
            if (onIdle != null)
            {
                onIdle.Invoke();
            }
        }
    }
    
    private void HandleMovment()
    {
        if (movementStopped)
        {
            return;
        }
        
        Vector3 vectorToDest = agent.nextPosition - transform.position;
        
        // Project the vector to the destination on forward and right axis
        float xMovement = Vector3.Dot(transform.right, vectorToDest);
        float zMovement = Vector3.Dot(transform.forward, vectorToDest);
        
        Vector2 deltaMovement = new Vector2(xMovement, zMovement);
        
        // Smooth out this movement by interpolating from the last
        // frame's movement to this one.
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPos = Vector2.Lerp(smoothDeltaPos, deltaMovement, smooth);

        Vector2 velocity = smoothDeltaPos / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.stoppingDistance;

        anim.SetBool(WalkBoolName, shouldMove);
        anim.SetFloat(ForwardSpeedParam, velocity.y);
        anim.SetFloat(LateralSpeedParam, velocity.x);
        
        // Don' t let the character exit the nav mesh agent's bounds
        if (vectorToDest.magnitude > agent.radius)
        {
            transform.position = 
                Vector3.Lerp(transform.position, agent.nextPosition, Time.deltaTime / 0.15f);
        }

        HandleMovementEvents(shouldMove);
    }

    public void OnAnimatorMove()
    {
        Vector3 animatorPosition = anim.rootPosition;

        animatorPosition.y = agent.nextPosition.y;
        transform.position = animatorPosition;
    }
}
