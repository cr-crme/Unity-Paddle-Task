using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Class to have change the emmisive of a material so it pulses in intensity 
/// </summary>
public class PulsingEmmisiveMap : MonoBehaviour
{

    /// Speed of the pulsing
    public float frequency = 1 ; 

    ///Max intensity of the material 
    public float maxEmissionIntensity = 1;

    ///Set default color for the emissive 
    public Color emmisiveColor = Color.white ;
    
    ///Vector color used to change the intensity 
    Vector4 emmisiveIntensity;

    ///Value used to change the value of the component of the emmisiveIntensity Vector4  
    float sinValue;

    ///Variables to have animation play on pause. Use those value instead of Time.time or Time.Deltatime which are dependant on timeScale 
    private float customTime = 0;
    private float timeIncrement = 0.01f;

    /// <summary>
    /// Call the method to change the emmisive every frame
    /// </summary>
    void Update()
    {
        emmisionChange();
    }

    /// <summary>
    /// Method to change the component of the vector4 used to set emissive color 
    /// Change the r,g,b,a component of the _emmisionColor over time adding an emmisiveColor to the present emisiveMap (if any) 
    /// </summary>
    void emmisionChange() 
    {
        //so the animation would continue while game is on pause
        if (Time.timeScale < 0.05)
        {
            customTime += timeIncrement;
        }

        else
        {
            customTime = Time.time;
        }

        sinValue = Mathf.Abs(Mathf.Sin(frequency * customTime));
        emmisiveIntensity = new Vector4((sinValue+emmisiveColor.r )* maxEmissionIntensity, (sinValue + emmisiveColor.g) * maxEmissionIntensity, (sinValue + emmisiveColor.b) * maxEmissionIntensity, emmisiveColor.a);

        this.GetComponent<Renderer>().material.SetColor("_EmissionColor", emmisiveIntensity);
    }

}
