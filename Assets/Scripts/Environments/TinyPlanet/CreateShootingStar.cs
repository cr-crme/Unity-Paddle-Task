using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A class  to spawn, every random time in seconds, ( between spawnFrequencyMax and spawnFrequencyMin)  a prefab of a shooting star
/// </summary>

public class CreateShootingStar : MonoBehaviour
{

    public bool randomSpawn = false; 
     
    //If random spawn used
    ///Max and min time for a new shootingStar prefab to be spawned.
    public float spawnFrequencyMax= 5;
    public float spawnFrequencyMin = 1;

    ///The prefab to be spawned
    public GameObject myShootingStar; 

    ///Game Time at which a prefab is spawned.  
    float spawnTime;

    /// <summary>
    /// Set the initial time to calculate when next prefab is gonna be spawned 
    /// </summary>
    void Start()
    {
        spawnTime = Time.time;
    }

    
    void Update()
    {
        if (randomSpawn == true)
        {
            randomTimeSpawn();
        }
    }

    /// <summary>
    /// Method to spawn a prefab "myShootingStar" at position (0,0,0) and with the default rotation of the prefab. 
    /// </summary>
    public void spawnShootingStar() 
    {
            Instantiate(myShootingStar, new Vector3(0, 0, 0), Quaternion.identity);
    }

    /// <summary>
    /// Every *random range* seconds spawn a prefab and reset the time variable spawnTime for next random spawn time in range
    /// </summary>
    private void randomTimeSpawn()
    {
        if (Time.time > spawnTime + Random.Range(spawnFrequencyMin, spawnFrequencyMax))
        {
            spawnShootingStar();
            spawnTime = Time.time;
        }
    }
}


