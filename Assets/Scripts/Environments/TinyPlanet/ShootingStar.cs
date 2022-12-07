using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A class to set a random life time in range , random direction, random speed and random start Position
/// for a gameObject
/// </summary
public class ShootingStar : MonoBehaviour
{

    ///Vector direction of a gameObject and its up vector 
    Vector3 direction;
    Vector3 upVect;

    //Vector position of the gameObject
    Vector3 position;


    ///Min life time and max life time for random life  in range 
    public float lifeMax = 10  ; //in seconds 
    public float lifeMin  = 5 ; //in seconds
    public  float life; //in seconds

    ///Time variable to calculate life time 
    float time; 

   
    ///Speed min and max for random speed in range 
    public float speedMin = 0.1f;
    public float speedMax = 2;
    float speed;



    /// <summary>
    /// Set the random values for life, speed , to start position and to the direction of the shooting star
    /// Set the position of the gameObject to the random position
    /// Set the direction of the gameObject to the ranbom direction
    /// Get the Up vector of the direction vector
    /// </summary>
    void Start()
    {
        
        life = Random.Range(lifeMin, lifeMax);

        speed = Random.Range(speedMin, speedMax);

        position = randomStartPosition(-60, 60, -30, 30, 50, 250);
        this.transform.position = position;

        direction = randomVector(0, 360);
        this.transform.rotation = new Quaternion(direction.x, direction.y, direction.z, 1);

        upVect = this.transform.up.normalized;

        time = Time.time;
    }

    /// <summary>
    /// Call the methods for moving the shootingStar and the timer method for destroying it after a time
    /// </summary> 
    void Update()
    {
        shootStar();
        timer();
    }

    /// <summary>
    /// Method to move the gameObject in the direction of the up vector of the direction.   
    /// </summary>  
    void shootStar()
    {
        position = transform.position;
        this.transform.position = new Vector3(position.x + upVect.x * speed, position.y + upVect.y * speed, position.z + upVect.z * speed);
    }


    /// <summary>
    /// Method to get random Values in range to create a vector3 
    /// </summary> 
    private Vector3 randomVector(float min, float max)
    {
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);
        float z = Random.Range(min, max);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Method method to get a start position between range
    /// And z is always positive (to not spawn below sight of player) 
    /// </summary> 
        private Vector3 randomStartPosition(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            float z = Random.Range(minZ, maxZ);
        
            return new Vector3(x, y, z);
        }

    /// <summary>
    /// Method to destroy the gameObject when the lifetime is done
    /// </summary>
    void timer() 
    {
        if (Time.time > time + life) 
        {
            Destroy(gameObject);
        }
    }
}