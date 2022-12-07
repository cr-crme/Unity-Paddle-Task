using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Class to give a GameObject a rotation on itself and an orbit around a point or another GameObject
/// If no speed are set , no rotation will happen
/// If no gameObject are selected in editor, the object will orbit around a orbitCenter
/// By default the radius will be the distance of this gameObject from orbit center
/// </summary>
public class OrbitingPlanet : MonoBehaviour
{

    ///Set gameObject refering self
    GameObject self;

    ///Speed of the rotation on itself
    public Vector3 rotatingSpeed = new Vector3( 0, 0, 0);

    ///Used to change uniformely all x,y,z components of rotationSpeed
    public float rotationSpeedFactor = 100;

    ///GameObject around wich self will orbit
    public GameObject orbitAroundWichGameObject;

    ///Will use this value as orbit center if no orbitAroundWichGameObject is defined
    public Vector3 orbitCenter = new Vector3();

    ///Speed of the orbit 
    public Vector3 orbitingSpeed = new Vector3(0, 0, 0);

    ///Used to change uniformely all x,y,z components of orbitingSpeed
    public float orbitingSpeedMultiplier = 100;

    ///Used to find the axis of orbit rotation
    Vector3 orbitingAxis = new Vector3();

    ///Variables to have animation play on pause. Use those value instead of Time.time or Time.Deltatime which are dependant on timeScale 
    private float customTime = 0;
    private float timeIncrement = 0.01f;


    /// <summary>
    /// Set self as current gameObject
    /// Use the orbitingSpeed to set the orbiting axis
    /// </summary>
    void Start()
    {
        self = this.gameObject;
        orbitingAxis = orbitingSpeed.normalized;
    }

    /// <summary>
    /// Call every frame the methods responsible for rotation the object on itself 
    /// And the method used to orbit the object around another
    /// </summary>
    void Update()
    {
        rotate();
        orbit();
    }


    /// <summary>
    /// Method to every frame set the rotation of the this gameObject by a value corresponding to the rotationSpeed/Speed multiplier
    /// </summary>
    public void rotate() {

        self.transform.Rotate(rotatingSpeed.x/ rotationSpeedFactor, rotatingSpeed.y/ rotationSpeedFactor, rotatingSpeed.z/ rotationSpeedFactor); 
        
    }

 
    /// <summary>
    /// Method to, every frame, Method that sets a new position to the object according to the rotation around another gameObject
    /// If a gameObject is selected as orbit center, this gameObject will orbit around it
    /// Else it will orbit around the orbit center 
    /// </summary>
    public void orbit()
    {

        //To have the animation play even when game is paused. 
        if (Time.timeScale < 0.05)
        {
            customTime = timeIncrement;
        }

        else
        {
            customTime = Time.deltaTime;
        }


        //orbiting movement
        if (orbitAroundWichGameObject != null) {

            self.transform.RotateAround(orbitAroundWichGameObject.transform.position, orbitingSpeed, orbitingSpeed.magnitude / orbitingSpeedMultiplier * customTime);
        }

        else { self.transform.RotateAround(orbitCenter, orbitingAxis , orbitingSpeed.magnitude *  orbitingSpeedMultiplier * customTime); }
    }
}
