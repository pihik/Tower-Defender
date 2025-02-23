using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
[RequireComponent(typeof(SphereCollider))]
public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] float maxDistanceToShoot = 15;

    SphereCollider myCollider;
    ParticleSystem projectilesVFX;
    ParticleSystem.EmissionModule emissionModule;

    List<GameObject> overlapsingEnemies = new List<GameObject>();

    void Start()
    {
        projectilesVFX = GetComponentInChildren<ParticleSystem>();
        myCollider = GetComponent<SphereCollider>();

        if (!projectilesVFX || !myCollider)
        {
            Debug.LogError("[TargetLocator::Start] Something went wrong on: " + name);
            return;
        }

        emissionModule = projectilesVFX.emission;
        emissionModule.enabled = false;
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
        transform.LookAt(ClossestEnemy().transform);
    }

    void Shoot()
    {
        Aim();
        emissionModule.enabled = true;
    }

    GameObject ClossestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = maxDistanceToShoot;

        foreach (var enemy in overlapsingEnemies)
        {
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
