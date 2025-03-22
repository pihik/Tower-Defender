using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Tower))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TargetLocator : MonoBehaviour
{
	public Action OnShoot;

	[SerializeField] Transform weapon;

	Tower tower;
	Rigidbody myRigidbody;
	SphereCollider myCollider;
	ProjectileScript projectileScript;

	protected DefenderStats stats;
	protected ParticleSystem projectilesVFX;
	protected float lastShotTime = 0f;

	List<GameObject> overlapsingEnemies = new List<GameObject>();

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

		myCollider.isTrigger = true;
		myCollider.includeLayers = InGameHelper.instance.GetEnemyLayer();
		myCollider.excludeLayers = ~InGameHelper.instance.GetEnemyLayer();
	}

	void InitializeTowerStats()
	{
		tower = GetComponent<Tower>();

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
		if (overlapsingEnemies.Count > 0 && !tower.IsBuilding())
		{
			Aim();
			return;
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
		GameObject enemy = ClosestEnemy();

		if (!enemy)
		{
			return;
		}

		Vector3 direction = enemy.transform.position - weapon.transform.position;
		direction.y = 0;
		weapon.transform.rotation = Quaternion.LookRotation(direction);

		if (CanShoot())
		{
			Shoot();
		}
	}

	bool CanShoot()
	{
		return Time.time - lastShotTime >= stats.attackSpeed;
	}

	protected virtual void Shoot()
	{
		OnShoot?.Invoke();
		lastShotTime = Time.time;
		projectilesVFX.Emit(1);
	}

	GameObject ClosestEnemy()
	{
		GameObject closestEnemy = null;
		float closestDistance = float.MaxValue;

		for (int i = overlapsingEnemies.Count - 1; i >= 0; i--)
		{
			var enemy = overlapsingEnemies[i];

			if (!enemy || !enemy.activeInHierarchy)
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
