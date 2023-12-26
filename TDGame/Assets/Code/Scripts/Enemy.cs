using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyScriptableObject enemyData;
    public List<Transform> SpawnPointTransforms {get; set;}

    public void TakeDamage() {
        enemyData.health -= enemyData.damage;
        if (enemyData.health <= 0) {
            Die();
        }
    }

    public void Die() {
        DeathAnimation();
        Destroy(gameObject);
    }

    public void DeathAnimation() {
        Debug.Log("Enemy Destroyed");
    }

    public void Attack() {

    }

    public int Move(List<Transform> waypoints, int waypointIndex, Transform character) {
        // Make sure we have not reached the last waypoint
        if (waypointIndex < waypoints.Count) {
            if (character.transform.position == waypoints[waypointIndex].transform.position) {
                // Check if we are at the waypoint yet; if so, advance the loop to the next item
                return waypointIndex + 1;
            } else {
                // Move to the current waypoint in the list
                character.transform.position = Vector2.MoveTowards(character.transform.position, waypoints[waypointIndex].position, Time.deltaTime * enemyData.moveSpeed);
            }
        }
        else {
            // Stop moving if we reached the last waypoint
            return -1;
        }
        return 0;
    }
}
