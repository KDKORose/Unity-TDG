using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicController : MonoBehaviour
{
    public Transform waypointManager;
    private List<Transform> waypoints = new List<Transform>();
    public int WaypointIndex {get; set;}
    private bool isMoving = true;
    public Enemy enemyClass;

    // Start is called before the first frame update
    void Start()
    {
        waypointManager = GameObject.FindGameObjectWithTag("Waypoint").transform;
        gameObject.GetComponent<SpriteRenderer>().sprite = enemyClass.enemyData.image;
        // loop through the children of "WaypointManager" and add them to a list of waypoints
        foreach (Transform waypoint in waypointManager)
        {
            waypoints.Add(waypoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only move if we have not reached the end of the path
        if (isMoving) {
            int moveInt = enemyClass.Move(waypoints, WaypointIndex, gameObject.transform);
            if (moveInt == -1) {
                isMoving = false;
            } else if (moveInt > 0) {
                WaypointIndex = moveInt;
            }
        }
    }
}
