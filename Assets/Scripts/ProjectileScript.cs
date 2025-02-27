using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class ProjectileScript : MonoBehaviour
{
    int damage = 1;

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("particle hit");
        if (other.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            Debug.Log("enemy is taking damage");
            enemyComponent.TakeDamage(damage);
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
