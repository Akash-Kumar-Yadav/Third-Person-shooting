using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playershooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    public Transform SpawnPoint;
    public float FireRate;
    private float CurrentTime;
    public float speed;

    List<GameObject> poolingObjects = new List<GameObject>();
    public int amountOfObjects;

    private void Start()
    {
       // Instantiate(muzzleFlash, SpawnPoint.transform.position, Quaternion.identity);
        CurrentTime = FireRate;
        for (int i = 0; i < amountOfObjects; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab) as GameObject;
           
            bullet.SetActive(false);
           
            poolingObjects.Add(bullet);
        }
    }

    private void Update()
    {
        CurrentTime += Time.deltaTime;
        for (int i = 0; i < poolingObjects.Count; i++)
        {
            if (!poolingObjects[i].activeInHierarchy)
            {
                if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
                {
                    muzzleFlash.Play();
                    poolingObjects[i].transform.position = SpawnPoint.position;
                    poolingObjects[i].transform.rotation = Quaternion.identity;
                    poolingObjects[i].SetActive(true);
                    Rigidbody rb = poolingObjects[i].GetComponent<Rigidbody>();
                    rb.velocity = transform.forward * speed;
                    break;
                }
                CurrentTime = 0;
            }
        }

       
    }
    private void OnDisable()
    {
      //  Destroy(muzzleFlash.gameObject);
    }



}
