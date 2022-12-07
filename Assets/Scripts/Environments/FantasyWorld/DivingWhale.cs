using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that set the behaviour of a diving whale
/// sets its rotation and sets its forward mouvement 
/// set a random life in range and a speed 
/// </summary>
public class DivingWhale : MonoBehaviour
{

    public float lifeMax = 10; //in seconds 
    public float lifeMin = 5; //in seconds
    public float life; //in seconds

    private GameObject self;

    private Quaternion rotation;
    private Vector3 position;
    private Vector3 direction;

    private float speedRotation = 0.05f;
    private float speedForward;

    //Variables to timescale == 0 
    private float customTime = 0;
    private float timeIncrement= 0.01f;

    private Animation diveAnim;


/// <summary>
/// Set the starting values and starting position and rotation
/// </summary>
    void Start()
    {
        self = this.gameObject;

        diveAnim = self.GetComponent<Animation>();

        life = Random.Range(lifeMin, lifeMax);
        speedForward = life / 200;
        speedRotation = speedForward * 5;


        self.transform.Rotate(80, Random.Range(0, 360), 0);

        position = randomStartPosition(-11, 40, 20, 125);
        self.transform.position = position;

        direction = -1 * self.transform.forward.normalized;
    }

    /// <summary>
    /// Find the forward vector of the object and assign that value to the direction of the object
    /// Destroy the object when its life is reached
    /// </summary>
    void Update()
    {
        //to have the animation play even when game is paused (time.timescale =0 ) 
        AnimationState currentState = diveAnim["dive"];
        currentState.time += Time.unscaledDeltaTime;
        diveAnim.Sample();

        direction = -1 * self.transform.forward.normalized;

        moveWhaleForward();
        rotateWhale();     

        customTime += timeIncrement;
        if (customTime >= life)
        {
            Destroy(self);
        }
    }

    /// <summary>
    /// Move the object according to the direction vector
    /// </summary>
    void moveWhaleForward()
    {
        position = transform.position;
       
        this.transform.position = new Vector3(position.x + direction.x * speedForward, position.y + direction.y * speedForward, position.z + direction.z * speedForward);
    }

  
    /// </summary>
    void rotateWhale()
    {
       rotation = self.transform.rotation;

       this.transform.Rotate(-speedRotation, 0, 0);    
    }


    /// <summary>
    /// Method method to get a start position between range
    /// </summary> 
    private Vector3 randomStartPosition(float minX, float maxX, float minZ, float maxZ)
    {
        float x = Random.Range(minX, maxX); 
        float y = -14f;
        float z = Random.Range(minZ, maxZ);

        return new Vector3(x, y, z);
    }
}
