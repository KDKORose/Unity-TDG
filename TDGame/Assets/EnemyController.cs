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
        foreach (Transform waypoint in waypointManager)
        {
            waypoints.Add(waypoint);
        }
        transform.position = waypoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) {
            Move();
        }
    }

    private void Move() {
        if (waypointIndex < waypoints.Count) {
            if (transform.position == waypoints[waypointIndex].transform.position) {
                waypointIndex++;
            } else {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, Time.deltaTime * moveSpeed);
            }
        }
        else {
            isMoving = false;
        }
    }
}
