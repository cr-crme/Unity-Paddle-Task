using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that sets the shader parameter in the scene to the starting wind level 
/// </summary>

public class WindManager : MonoBehaviour
{
    //the wind level selected in the menu
    private int windLevel;

    //the multiplier that is gonna be applied to the shader
    private float windMultiplier;


    /// <summary>
    /// Get wind level from global preference
    ///Set wind multiplier to selected wind level
    ///Call method to change shader propertie
    /// </summary>
    void Awake()
    {
        windLevel = GlobalPreferences.Instance.StartingWindLevel;

        switch (windLevel)
        {
            case 0:
                windMultiplier = windLevel;
                break;

            case 1:
                windMultiplier = .5f;
                break;

            case 2:
                windMultiplier = 1;
                break;

            case 3:
                windMultiplier = 2;
                break;
        }

        changeShaderWind();
    }

  

    /// <summary>
    /// Method that modifies shader's properties _windmultiplier. 
    /// When 0, stops all wind effect
    /// </summary>
    private void changeShaderWind()
    {
            Shader.SetGlobalFloat("_windMultiplier", windMultiplier);
    }

}
