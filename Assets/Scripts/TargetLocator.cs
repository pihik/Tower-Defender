using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] float maxDistanceToShoot = 15;
    //EnemyMover enemy;

    [SerializeField] int numberOfEnemies;
    [SerializeField] Transform ammunition;


    [SerializeField] bool inDistance = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //EnemyCounter();
        //Aiming();
        //Shoot();
        FindAimShoot();
    }/*
    void EnemyCounter()
    {
        //enemy = FindObjectOfType<EnemyMover>();
        numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length; //CountingEnemies
    }
    
    void Shoot()
    {
        if (numberOfEnemies > 0)
        {
            var ammunitionEmision = ammunition.GetComponent<ParticleSystem>().emission;
            ammunitionEmision.enabled = true;
        }
        else if (numberOfEnemies <= 0)
        {
            var ammunitionEmision = ammunition.GetComponent<ParticleSystem>().emission;
            ammunitionEmision.enabled = false;
        }
    }
    

    void Aiming()
    {
        if (numberOfEnemies > 0)
        {
            weapon.LookAt(enemy.transform.position);
        }
        else
        {
            return;
        }
    }*/
    
    void FindAimShoot()
    {
        numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemyMover[] enemies = FindObjectsOfType<EnemyMover>();
        
        foreach(EnemyMover enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (numberOfEnemies > 0 && distance < maxDistanceToShoot)
            {
                inDistance = true;
                weapon.LookAt(enemy.transform.position);
                var ammunitionEmmision = ammunition.GetComponent<ParticleSystem>().emission;
                ammunitionEmmision.enabled = true;
                return;
            }
            else if (distance > maxDistanceToShoot)
            {
                inDistance = false;
                var ammunitionEmmision = ammunition.GetComponent<ParticleSystem>().emission;
                ammunitionEmmision.enabled = false;
                
            }
            
        }
        if(numberOfEnemies <= 0)
        {
            inDistance = false;
            var ammunitionEmmision = ammunition.GetComponent<ParticleSystem>().emission;
            ammunitionEmmision.enabled = false;
        }

    }
}
