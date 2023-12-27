using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public TowerTurret tower;
    // Start is called before the first frame update
    void Start()
    {
        tower.onEnemyDetected.AddListener(OnEnemyDetected);
        tower.onEnemyLockedOn.AddListener(OnEnemyLockedOn);
        tower.onEnemyLost.AddListener(OnEnemyLost);
        tower.StartFiring();
    }

    // Update is called once per frame
    void Update()
    {
        tower.DetectEnemiesInRange();
        tower.RotateTowardsEnemy();
    }

    void OnEnemyDetected() {
        Debug.Log("Enemy detected");
    }

    void OnEnemyLockedOn() {
        Debug.Log("Enemy locked on!");
        tower.RotateTowardsEnemy();
    }

    void OnEnemyLost() {
        Debug.Log("Enemy out of range.");
    }
}
