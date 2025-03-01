using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Tower))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;

    DefenderStats stats;
    Rigidbody myRigidbody;
    SphereCollider myCollider;
    ProjectileScript projectileScript;

    ParticleSystem projectilesVFX;
    ParticleSystem.EmissionModule emissionModule;

    List<GameObject> overlapsingEnemies = new List<GameObject>();

    float viewRange = 10f;

    void Awake()
    {
        InitializeTowerStats();
        projectileScript = GetComponentInChildren<ProjectileScript>();

        if (!projectileScript)
        {
            Debug.LogError("[TargetLocator::Start] ProjectileScript is missing");
            return;
        }

        InitializeProjectileVariables();

        myCollider = GetComponent<SphereCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        projectilesVFX = projectileScript.GetComponent<ParticleSystem>();

        if (!projectilesVFX || !myCollider || !myRigidbody)
        {
            Debug.LogError("[TargetLocator::Start] Something went wrong on: " + name);
            return;
        }

        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;

        emissionModule = projectilesVFX.emission;
        emissionModule.enabled = false;

        myCollider.isTrigger = true;
        myCollider.includeLayers = InGameHelper.instance.GetEnemyLayer();
        myCollider.excludeLayers = ~InGameHelper.instance.GetEnemyLayer();

        viewRange = myCollider.radius;
    }

    void InitializeTowerStats()
    {
        Tower tower = GetComponent<Tower>();

        if (!tower)
        {
            Debug.LogError("[TargetLocator::InitializeTowerStats] Tower is missing");
            return;
        }

        stats = tower.GetStats();
    }

    void InitializeProjectileVariables()
    {
        if (!projectileScript || !stats)
        {
            Debug.LogError("[TargetLocator::InitializeProjectileVariables] Something went wrong on: " + name);
            return;
        }

        projectileScript.InitializeProjectileStats(stats.damage, stats.attackSpeed);
    }

    void Update()
    {
        if (overlapsingEnemies.Count > 0)
        {
            Aim();
            return;
        }

        if (emissionModule.enabled)
        {
            emissionModule.enabled = false; // stops shooting
        }
    }

    void OnTriggerEnter(Collider other)
    {
        int collisionLayerIndex = other.gameObject.layer;

        if ((InGameHelper.instance.GetEnemyLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex)
        {
            if (!overlapsingEnemies.Contains(other.gameObject))
            {
                overlapsingEnemies.Add(other.gameObject);
            }
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
        Debug.Log(name + " aiming to " + ClossestEnemy());

        if (!enemy)
        {
            return;
        }

        Vector3 direction = enemy.transform.position - weapon.transform.position;
        direction.y = 0;
        weapon.transform.rotation = Quaternion.LookRotation(direction);

        Shoot();
    }

    void Shoot()
    {
        //adjust angle or gravity or whatever to hit the enemy
        Debug.Log(ClossestEnemy().name + " is being shot");
        emissionModule.enabled = true;
    }

    GameObject ClossestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

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
