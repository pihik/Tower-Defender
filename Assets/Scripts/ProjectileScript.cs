using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ProjectileScript : MonoBehaviour
{
	int damage = 1;

	ParticleSystem particleComponent;
	ParticleSystem.EmissionModule emissionModule;

	void Awake()
	{
		InitializeParticleComponent();
	}

	void OnParticleCollision(GameObject other)
	{
		if ((InGameHelper.instance.GetEnemyLayer() & (1 << other.layer)) != 0)
		{
			if (other.TryGetComponent<Enemy>(out Enemy enemyComponent))
			{
				enemyComponent.TakeDamage(damage);
			}
		}
	}

	public void InitializeProjectileStats(int damage, float attackSpeed)
	{
		InitializeParticleComponent();

		this.damage = damage;
		emissionModule.rateOverTime = attackSpeed;
	}

	public ParticleSystem GetParticleSystem()
	{
		return particleComponent;
	}

	void InitializeParticleComponent()
	{
		if (particleComponent)
		{
			return;
		}

		particleComponent = GetComponent<ParticleSystem>();

		if (!particleComponent)
		{
			Debug.LogError("[ProjectileScript::Awake] Missing ParticleSystem component");
			return;
		}

		emissionModule = particleComponent.emission;
	}
}
