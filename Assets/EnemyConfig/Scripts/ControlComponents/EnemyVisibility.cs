using System;
using UnityEditor;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    public Player target;
    
    public float detectionRange = 10f;
    public float viewRange = 15;
    public float viewAngle = 45.0f;
    // If the player gets really close to the enemy, the enemy doesn't "see" him anymore
    // This case should be treated so the enemy keeps shooting the player
    public float deadAngleRange = 0.8f;
    
    // If the target is detected, then it is in the detection range
    public bool IsTargetDetected { get; private set; }
    // If the target is visible then it is in the view range and this game object is also rotated towards the target
    public bool IsTargetVisible { get; private set; }

    private void Start()
    {
        IsTargetVisible = false;
        IsTargetDetected = false;
    }

    public EnemyVisibility SetTargetReference(Player player)
    {
        target = player;

        return this;
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckVisibility();
    }

    // Detection range is larger then the view range
    // Also the player is being detected even if it is behind the enemy
    private void CheckVisibility()
    {
        if (target == null)
        {
            return;
        }
        
        var thisTransform = transform;
        var directionToTarget = target.transform.position - thisTransform.position;
        
        // Check the distance to the target
        if (directionToTarget.magnitude <= detectionRange)
        {
            IsTargetDetected = true;
            
            // Treat the dead angle case
            if (directionToTarget.magnitude <= deadAngleRange)
            {
                IsTargetVisible = true;
                return;
            }
        }

        var angleToTarget = Vector3.Angle(thisTransform.forward, directionToTarget);

        if (angleToTarget > viewAngle / 2)
        {
            IsTargetVisible = false;
            return;
        }
        
        // Now make sure a ray from the enemy's "eyes" to the target is not obstructed by an obstacle
        RaycastHit hit;
        var ray = new Ray(thisTransform.position, directionToTarget);
        
        if (Physics.Raycast(ray, out hit, viewRange))
        {
            if (hit.collider.transform == target.transform)
            {
                IsTargetVisible = true;
                return;
            }
        }

        IsTargetVisible = false;
    }
    
    // Visual feedback in the scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

// Code taken from the Unity Game Development Cookbook chapter 10 for editing the visible range.
[CustomEditor(typeof(EnemyVisibility))]
public class EnemyVisibilityEditor : Editor {

    // Called when Unity needs to draw the Scene view.
    private void OnSceneGUI()
    {
        // Get a reference to the EnemyVisibility script we're
        // looking at
        var visibility = target as EnemyVisibility;

        if (visibility == null)
        {
            return;
        }

        // Start drawing at 10% opacity
        Handles.color = Color.white;

        // Drawing an arc sweeps from the point you give it. We want to
        // draw the arc such that the middle of the arc is in front of
        // the object, so we'll take the forward direction and rotate
        // it by half the angle.

        var forwardPointMinusHalfAngle =
            // rotate around the y-axis by half the angle
            Quaternion.Euler(0, -visibility.viewAngle / 2, 0)
                      // rotate the forward direction by this
                      * visibility.transform.forward;

        // Draw the arc to visualize the visibility arc
        Vector3 arcStart =
            forwardPointMinusHalfAngle * visibility.viewRange;

        Handles.DrawSolidArc(
            visibility.transform.position, // Center of the arc
            Vector3.up,                    // Up direction of the arc
            arcStart,                      // Point where it begins
            visibility.viewAngle,              // Angle of the arc
            visibility.viewRange         // Radius of the arc
        );

        // Draw a scale handle at the edge of the arc; if the user drags
        // it, update the arc size.

        // Reset the handle color to full opacity
        Handles.color = Color.white;

        // Compute the position of the handle, based on the object's
        // position, the direction it's facing, and the distance
        Vector3 handlePosition =
            visibility.transform.position +
                  visibility.transform.forward * visibility.viewRange;

        // Draw the handle, and store its result.
        visibility.viewRange = Handles.ScaleValueHandle(
            visibility.viewRange,         // current value
            handlePosition,                 // handle position
            visibility.transform.rotation,  // orientation
            1,                              // size
            Handles.ConeHandleCap,          // cap to draw
            0.25f);                         // snap to multiples of this
                                            // if the snapping key is
                                            // held down
    }
}
