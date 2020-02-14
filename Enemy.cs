
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Ai range and waypoints")]
    [Range(0, .9f)]
    [SerializeField] private float alert = .3f;
    public float rangeRadius; // its the radius of the enemy
    public float rangeRadius1; // its the radius of the enemy
    public Transform eyes; // eye transform is the head of the ai
    public Transform target; // its the player
    public Transform[] waypouints;
    NavMeshAgent agent;
    public float fieldOfView; // its the black line increase the enemy eye sight

    [Header("Speed Values")]
    [SerializeField] private float ChaseSpeed;
    [SerializeField] private float PatrolSpeed;
    [SerializeField] private float turnSpeed;

    [Header("Shooting")]
    public GameObject Bullet;
    public GameObject spawnPoint;
    public float FireRate;
    private float fire;
    public int BulletSpeed;


    [Header("ObjectPooling")]
    List<GameObject> ObjectPooling = new List<GameObject>();
    public int NumberOfBullets;


    private Animator animator;
   // public int alertAngle;
     public int index; // waypoint index
    int proity; // its for switch

    private void Start()
    {
        fire = FireRate;
        proity = 1; // for patrol
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        for (int i = 0; i < NumberOfBullets; i++)
        {
            GameObject pool = Instantiate(Bullet) as GameObject;
            pool.SetActive(false);
            ObjectPooling.Add(pool);
        }
    }

    private void Update()
    {
        fire += Time.deltaTime;

        Vector3 direction = target.position - transform.position;

        Vector3 angleRotate = Quaternion.AngleAxis(fieldOfView, Vector3.up) * transform.forward * rangeRadius1;
        float angle = Vector3.Angle(transform.forward, direction);

        if (Vector3.Distance(transform.position, target.position) <= rangeRadius1)
        {
           
            print("In Ranage");
            if (angle <= fieldOfView)
            {
                Debug.LogWarning("can see target");
                chase(direction);
                proity = 2; // for chase
            }
        }
        if (Vector3.Distance(transform.position, target.position) <= rangeRadius)
        {
            proity = 3;
            agent.isStopped = true;
            if (angle <= fieldOfView)
            {
                Debug.LogWarning("shoot target");
                shoot();
            }
        }
        else if (Vector3.Distance(transform.position, target.position) <= rangeRadius1)
        {
            agent.isStopped = false;
            if (angle <= fieldOfView)
            {
                Debug.LogWarning("can see target");
                chase(direction);
                proity = 2; // for chase
            }
            else
            {
                proity = 1;
            }

        } else
        {
            agent.isStopped = false;
            proity = 1;
           
        }

        switch (proity)
        {
            case 1:
                patrol();
                break;
            case 2:
                chase(direction);
                break;
            case 3:
                alertNpc();
                break;
            default:
                break;
        }
    }

    private void shoot()
    {
        for (int i = 0; i < ObjectPooling.Count; i++)
        {
            if (!ObjectPooling[i].activeInHierarchy)
            {
                if (fire >= FireRate)
                {
                    //GameObject newBullet = Instantiate(Bullet) as GameObject;
                    //newBullet.transform.position = spawnPoint.transform.position;
                    //newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * BulletSpeed);
                    ObjectPooling[i].transform.position = spawnPoint.transform.position;
                    ObjectPooling[i].transform.rotation = Quaternion.identity;
                    ObjectPooling[i].SetActive(true);
                    Rigidbody rb = ObjectPooling[i].GetComponent<Rigidbody>();
                    rb.velocity = transform.forward * BulletSpeed;
                    fire = 0;
                    break;

                    
                }
            }
        }

       
    }

    private void alertNpc()
    {

        animator.SetBool("patrol", false);
        animator.SetBool("chase", false);

        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        if (transform.rotation.y > alert)
        {
            turnSpeed -= turnSpeed;
            turnSpeed = -30;
        }
        else if (transform.rotation.y < -alert)
        {
            turnSpeed -= turnSpeed;
            turnSpeed = 30;
        }

    }

    private void chase(Vector3 direction)
    {
        lookAtTarget(direction);  // local method
      //  transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * ChaseSpeed);
        agent.speed = ChaseSpeed;
        agent.SetDestination(target.position);
        animator.SetBool("patrol", false);
        animator.SetBool("chase", true);

        if (Vector3.Distance(transform.position, target.position) <= 2f)
        {
          
            animator.SetBool("patrol", false);
            animator.SetBool("chase", false);
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    void patrol()
    {
        animator.SetBool("patrol", true);
        animator.SetBool("chase", false);
        // transform.position = Vector3.MoveTowards(transform.position, waypouints[index].position, Time.deltaTime * PatrolSpeed);
        agent.speed = PatrolSpeed;
        agent.SetDestination(waypouints[index].position);
        Vector3 lookAt = waypouints[index].position - transform.position;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), Time.deltaTime * 5);
       
        if (Vector3.Distance(transform.position, waypouints[index].position) <= .2f)
        {
            index++;
        }
        
        if (index == waypouints.Length)
        {
            index = 0;
        }
    }

    private void lookAtTarget(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
       
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
        
    }

    // what ever written in this method is to display on the scene it has nothing to do with game scene.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangeRadius1);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * rangeRadius);
        Vector3 angleRotate = Quaternion.AngleAxis(fieldOfView, Vector3.up) * transform.forward * rangeRadius1;
        Vector3 angleRotate1 = Quaternion.AngleAxis(-fieldOfView, Vector3.up) * transform.forward * rangeRadius1;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, angleRotate);
        Gizmos.DrawRay(transform.position, angleRotate1);
        if (Vector3.Distance(transform.position, target.position) <= rangeRadius)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, target.position - transform.position);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, target.position - transform.position);
        }

    }

}

