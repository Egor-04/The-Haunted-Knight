using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamageSphere : MonoBehaviour
{
    [SerializeField] private float _discardingForce = 10f;
    [SerializeField] private float _stunTime = 0f;
    [SerializeField] private int _minDamage = 5, _maxdamage = 25;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.CompareTag("Enemy"))
        {
            Vector2 directionDiscarding = collision2D.transform.position - transform.position;
            StunDiscarding(collision2D.collider, directionDiscarding);

            Health health = collision2D.collider.GetComponent<Health>();
            health.Damage(_minDamage, _maxdamage);
        }

        if (collision2D.collider.CompareTag("Bullet") || collision2D.collider.CompareTag("Player"))
        {
            return;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void StunDiscarding(Collider2D collider, Vector2 directionDiscarding)
    {
        Enemy enemyScript = collider.GetComponent<Enemy>();
        enemyScript.StunEnemy(_stunTime);

        Rigidbody2D enemyRb2 = collider.GetComponent<Rigidbody2D>();
        enemyRb2.AddForce(directionDiscarding * _discardingForce);
    }
}
