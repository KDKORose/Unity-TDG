using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform waypointManager;
    private List<Transform> waypoints = new List<Transform>();
    private int waypointIndex = 0;
    private bool isMoving = true;
    public int moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // loop through the children of "WaypointManager" and add them to a list of waypoints
        foreach (Transform waypoint in waypointManager)
        {
            waypoints.Add(waypoint);
        }

        // Set initial position of enemy character to the first waypoiunt
        transform.position = waypoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Only move if we have not reached the end of the path
        if (isMoving) {
            Move();
        }
    }

    private void Move() {
        // Make sure we have not reached the last waypoint
        if (waypointIndex < waypoints.Count) {
            if (transform.position == waypoints[waypointIndex].transform.position) {
                // Check if we are at the waypoint yet; if so, advance the loop to the next item
                waypointIndex++;
            } else {
                // Move to the current waypoint in the list
                transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, Time.deltaTime * moveSpeed);
            }
        }
        else {
            // Stop moving if we reached the last waypoint
            isMoving = false;
        }
    }
}
