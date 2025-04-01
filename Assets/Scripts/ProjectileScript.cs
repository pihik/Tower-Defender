using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ProjectileScript : MonoBehaviour
{
	int damage = 1;

	ParticleSystem particleComponent;
	ParticleSystem.EmissionModule emissionModule;

	bool dealtDamage = false;

	void Awake()
	{
		InitializeParticleComponent();
	}

	void OnParticleCollision(GameObject other)
	{
		if ((InGameHelper.instance.GetEnemyLayer() & (1 << other.layer)) != 0 &&
			other.TryGetComponent(out Enemy enemyComponent) && !dealtDamage)
		{
			enemyComponent.TakeDamage(damage);
			dealtDamage = true;
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

	public void ResetDealtDamage()
	{
		dealtDamage = false;
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
