using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class ProjectileScript : MonoBehaviour
{
    int damage = 1;

    void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(damage);
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
