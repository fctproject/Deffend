using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEye : MonoBehaviour
{
    public Transform closestEnemy;
    public GameObject[] multipleEnemies;
    public Tower tower;
    public bool shouldShoot;

    void Start()
    {
        tower = GetComponentInParent<Tower>();
    }

    void Update()
    {
        if (closestEnemy != null)
        {
            LookAtEnemy();
            if (shouldShoot)
            {
                tower.Shoot();
            }
        }
        else
        {
            closestEnemy = GetClosestEnemy(); // Update closest enemy if none is targeted
        }
    }

    public void LookAtEnemy()
    {
        Vector2 lookDirection = closestEnemy.transform.position - transform.position;
        transform.up = lookDirection.normalized; // Normalize direction
    }

    public Transform GetClosestEnemy()
    {
        // Find all enemies with the tag "Enemy"
        multipleEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform enemyPos = null;

        // Search through enemies to find the closest one
        foreach (GameObject enemies in multipleEnemies)
        {
            float currentDistance = Vector3.Distance(transform.position, enemies.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                enemyPos = enemies.transform;
            }
        }

        return enemyPos;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            shouldShoot = true;
            closestEnemy = GetClosestEnemy(); // Update closest enemy when one enters
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            shouldShoot = false; // Stop shooting when the enemy exits
            closestEnemy = null; // Clear the reference to the closest enemy
        }
    }
}
