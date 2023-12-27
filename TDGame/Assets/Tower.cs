using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    public TowerScriptableObject towerData;
    public LayerMask enemyLayerMask;
    private Transform targetEnemy;

    public UnityEvent onEnemyDetected;
    public UnityEvent onEnemyLockedOn;
    public UnityEvent onEnemyLost;
    private bool isTrackingEnemy = false;
    private SpriteRenderer lockedOnSpriteRenderer;
    public GameObject bulletPrefab;
    private Coroutine firingCoroutine;

    public virtual void Attack() {
        // WIA
    }

    public void DetectEnemiesInRange() {
        if (targetEnemy == null || !IsTargetValid(targetEnemy)) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, towerData.range, enemyLayerMask);
            //Debug.DrawLine(transform.position, transform.position + (transform.right * towerData.range), Color.black, 100f);

            Collider2D newTarget = FindTarget(colliders);

            if (newTarget != null && newTarget.transform != targetEnemy) {
                // Handle new target
                targetEnemy = newTarget.transform;

                // Start tracking enemy
                isTrackingEnemy = true;
                LockOnEnemy();
                onEnemyDetected.Invoke();
            }
            else if (targetEnemy != null && !IsTargetValid(targetEnemy)) {
                // Handle no target and stop tracking enemy
                isTrackingEnemy = false;
                onEnemyLost.Invoke();
                targetEnemy = null;
            }
        }
        
    }

    private bool IsTargetValid(Transform target) {
        float distance = Vector2.Distance(transform.position, target.position);
        return distance <= towerData.range;
    }

    public void LockOnEnemy() {
        if (targetEnemy != null) {
            // Reset previous enemy color
            if (lockedOnSpriteRenderer != null)
            {
                lockedOnSpriteRenderer.color = Color.white; // You can set it to the original color or any desired color
            }

            // Give new enemy color
            lockedOnSpriteRenderer = targetEnemy.GetComponent<SpriteRenderer>();
            if (lockedOnSpriteRenderer != null) {
                lockedOnSpriteRenderer.color = Color.black;
            }
            onEnemyLockedOn.Invoke();
        }
    }

    public void RotateTowardsEnemy() {
        if (targetEnemy != null) {
            Vector3 direction = targetEnemy.position - transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 180f;
            gameObject.transform.GetChild(1).gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.GetChild(1).gameObject.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle)), 300f * Time.deltaTime);
        }
        else {
            transform.rotation = Quaternion.identity;
        }
    }

    public Collider2D FindTarget(Collider2D[] colliders) {
        Collider2D closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance || (Mathf.Approximately(distance, closestDistance) && collider.GetComponent<Enemy>().GetCurrentHealth() < closestEnemy.GetComponent<Enemy>().GetCurrentHealth())) {
                closestEnemy = collider;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }

    public void FireBullet(Transform target) {
        Transform firePoint = gameObject.transform.GetChild(1).GetChild(1).gameObject.transform;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));

        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        if (bulletScript != null) {
            bulletScript.SetDamage(towerData.damage);
            bulletScript.target = target;
        }
    }

    IEnumerator FireDuringWave() {
        while (true) {
            if (targetEnemy != null) {
                FireBullet(targetEnemy);
                yield return new WaitForSeconds(towerData.fireRate);
            } else {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void StartFiring() {
        firingCoroutine = StartCoroutine(FireDuringWave());
    }
}
