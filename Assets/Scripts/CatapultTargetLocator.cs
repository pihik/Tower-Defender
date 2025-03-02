using System.Collections;
using UnityEngine;

public class CatapultTargetLocator : TargetLocator
{
    [SerializeField] Transform arm;
    [SerializeField] float shootAngle = 90f;
    [SerializeField] float resetAngle = 0f;
    [SerializeField] float animationTime = 0.5f;

    private bool isShooting = false;

    GameObject projectile;

    void Start()
    {
        if (!arm)
        {
            Debug.LogError("[CatapultTargetLocator::Start] Arm is missing");
            return;
        }

        projectile = arm.transform.GetChild(0).gameObject;

        if (!projectile)
        {
            Debug.LogError("[CatapultTargetLocator::Start] Projectile is missing");
            return;
        }
    }

    protected override void Shoot()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        yield return StartCoroutine(AnimateArm(shootAngle));

        base.Shoot();
        projectile.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(AnimateArm(resetAngle));
        yield return new WaitForSeconds(1 / stats.attackSpeed);

        isShooting = false;
    }

    private IEnumerator AnimateArm(float targetAngle)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = arm.localRotation;
        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0, 0);

        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            arm.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / animationTime);
            yield return null;
        }

        arm.localRotation = targetRotation;
        projectile.SetActive(true);
    }
}
