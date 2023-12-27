using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public Transform target;
    private int damage;
    public float rotateSpeed = 200;
    public Rigidbody2D rigidBody;

    public void SetDamage(int newDamage) {
        damage = newDamage;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null) {
            MoveBullet();
        } else {
            Destroy(gameObject);
        }
    }

    public void MoveBullet() {
        Vector2 direction = (Vector2)target.position - rigidBody.position;
        direction.Normalize();
        rigidBody.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Collision");
        if (collision.CompareTag("Enemy")) {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
