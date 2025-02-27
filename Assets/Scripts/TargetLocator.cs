using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;

    Rigidbody myRigidbody;
    SphereCollider myCollider;
    ProjectileScript projectileScript;
    ParticleSystem projectilesVFX;
    ParticleSystem.EmissionModule emissionModule;

    List<GameObject> overlapsingEnemies = new List<GameObject>();

    DefenderStats stats;

    void Awake()
    {
        projectileScript = GetComponentInChildren<ProjectileScript>();

        if (!projectileScript)
        {
            Debug.LogError("[TargetLocator::Start] ProjectileScript is missing");
            return;
        }

        projectilesVFX = projectileScript.GetComponent<ParticleSystem>();
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

    void Start()
    {
        projectileScript.SetDamage(stats.attack);
    }

    public void SetStats(DefenderStats stats)
    {
        this.stats = stats;

        InitializeViewRange();
    }

    void InitializeViewRange()
    {
        if (!myCollider)
        {
            return;
        }

        myCollider.radius = stats.viewRange;
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

        if (IsEnemyInRange(enemy))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        emissionModule.enabled = true;
        emissionModule.rateOverTime = stats.attackSpeed;
    }

    bool IsEnemyInRange(GameObject enemy)
    {
        if (!enemy)
        {
            return false;
        }
        return Vector3.Distance(transform.position, enemy.transform.position) <= stats.attackDistance;
    }

    GameObject ClossestEnemy()
    {
        if (!stats)
        {
            Debug.LogError("[TargetLocator::ClossestEnemy] Stats are missing");
            return null;
        }

        GameObject closestEnemy = null;
        float closestDistance = stats.viewRange;

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
