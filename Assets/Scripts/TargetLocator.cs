using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    float attackDistance = 15;

    Rigidbody myRigidbody;
    SphereCollider myCollider;
    ParticleSystem projectilesVFX;
    ParticleSystem.EmissionModule emissionModule;

    List<GameObject> overlapsingEnemies = new List<GameObject>();

    void Start()
    {
        projectilesVFX = GetComponentInChildren<ParticleSystem>();
        myCollider = GetComponent<SphereCollider>();
        myRigidbody = GetComponent<Rigidbody>();

        if (!projectilesVFX || !myCollider || !myRigidbody)
        {
            Debug.LogError("[TargetLocator::Start] Something went wrong on: " + name);
            return;
        }

        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;

        emissionModule = projectilesVFX.emission;
        emissionModule.enabled = false;
    }

    public void SetAttackDistance(float distance)
    {
        attackDistance = distance;
    }

    public void SetViewRange(float viewRange)
    {
        if (!myCollider)
        {
            return;
        }

        myCollider.radius = viewRange;
    }

    void Update()
    {
        if (overlapsingEnemies.Count > 0)
        {
            Shoot();
            return;
        }

        emissionModule.enabled = false; // stop shooting
    }

    void OnTriggerEnter(Collider other)
    {
        int collisionLayerIndex = other.gameObject.layer;
        Debug.Log("trigger active on: " + name + " triggered object is: " + other.name);

        if ((InGameHelper.instance.GetEnemyLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex)
        {
            overlapsingEnemies.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        int collisionLayerIndex = other.gameObject.layer;

        if ((InGameHelper.instance.GetEnemyLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex)
        {
            if (overlapsingEnemies.Contains(other.gameObject))
            {
                overlapsingEnemies.Remove(other.gameObject);
            }
        }
    }

    void Aim()
    {
        GameObject enemy = ClossestEnemy();

        if (!enemy)
        {
            return;
        }

        transform.LookAt(enemy.transform);
    }

    void Shoot()
    {
        Aim();
        emissionModule.enabled = true;
    }

    GameObject ClossestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = attackDistance;

        for (int i = overlapsingEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = overlapsingEnemies[i];
            if (!enemy)
            {
                overlapsingEnemies.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
