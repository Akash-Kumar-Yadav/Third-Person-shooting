using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamesc : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject spawnPointsForAnswer;
    public GameObject[] objectsToSpawn;

    GameObject gg;
    public List<GameObject> answer = new List<GameObject>();

    private void Start()
    {
        swap();
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           
            swap();
        }
      
    }

    private void swap()
    {
       
        for (int i = 0; i < spawnPoints.Length; i++)
        {
             gg = Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)], spawnPoints[i].transform.position, Quaternion.identity);
            answer.Add(gg);
            
        }

        Instantiate(answer[Random.Range(0, answer.Count)], spawnPointsForAnswer.transform.position, Quaternion.identity);
    }

    void remove()
    {
        var des = FindObjectOfType<destr>();
        des.des();
    }
}
