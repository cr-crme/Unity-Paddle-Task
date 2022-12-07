using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A class for having an object's oscillate on one axis over time 
/// </summary>
/// 


public class Oscillating : MonoBehaviour
{

    ///The maxDistance the object will travel
    [SerializeField]
    private float distanceJourney = 0;

    ///A speed multiplicator for the oscillation
    [SerializeField]
    private float speed = 0;

    ///The Axis that the object will follow during its oscillation. 
    [SerializeField]
    private OscillationDirection movementAxis;

    ///Vector position of the object
    Vector3 startPosition;
    Vector3 currentPosition;

    ///Variables to have animation play on pause. Use those value instead of Time.time or Time.Deltatime which are dependant on timeScale 
    private float customTime = 0;
    private float timeIncrement = 0.01f;

    /// <summary>
    /// Initialize the start position of the object
    /// </summary>
    void Start()
    {
        startPosition = this.transform.position;
    }

    /// <summary>
    /// Update the position of the object based on the distance * sine (time * speed) on the chosen axis
    /// This position is calculated in the oscilation method
    /// </summary>
    void Update()
    {
        currentPosition = oscilation(distanceJourney, speed, movementAxis);
        this.transform.position = currentPosition;
    }


    /// <summary>
    /// According to the chosen axis, add an offset to the position corresponding coordinate. 
    /// The offset is calculated over time by calculating the sine of (time * speed) and multplicated by the max distance. So that,
    /// when sin(time * speed) ==1 the offset is == maxDistance
    /// </summary>
    private Vector3 oscilation(float distance, float speed, OscillationDirection movementAxis)
    {

        //To have the animation play even when game is paused. 
        if (Time.timeScale < 0.05)
        {
            customTime += timeIncrement;
        }

        else
        {
            customTime = Time.time;
        }


        switch (movementAxis)
        {
            case OscillationDirection.x:
                currentPosition = new Vector3(startPosition.x + distance * Mathf.Sin(customTime * speed), startPosition.y, startPosition.z );
                break;

            case OscillationDirection.y:
                currentPosition = new Vector3(startPosition.x , startPosition.y + distance * Mathf.Sin(customTime * speed), startPosition.z);
                break;

            case OscillationDirection.z:
                currentPosition = new Vector3(startPosition.x, startPosition.y , startPosition.z + distance * Mathf.Sin(customTime *  speed));
                break;
        }

        return currentPosition; 

    }

    /// <summary>
    /// An enumeration of the axis.
    /// </summary>
    private enum OscillationDirection { x, y, z };
}
