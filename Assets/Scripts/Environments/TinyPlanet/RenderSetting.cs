using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A class for setting a Render Setting on load 
/// Used in the prefab of the environment to make sure that, on loading in the paddle scene 
/// the rendering is like the one intended
/// </summary>

public class RenderSetting : MonoBehaviour
{
    
    /// The skyBox Material to be set on load
    [SerializeField]
    private Material skyBoxMaterialToLoad;


    ///The light reflection intensity in the scene. Default set to 1. 
    [SerializeField]
    private float skyBoxLightIntensity = 1; 



    /// <summary>
    /// Call the methods responsible for setting the skybox material and setting the light reflection intensity . 
    /// </summary>
    private void Awake()
    {
        changeSkyBox();
        changeLightIntensity();
    }


    /// <summary>
    /// Change the render setting to set the skybox material. 
    /// </summary>
    private void changeSkyBox()
    {
        if (skyBoxMaterialToLoad != null)
        {
            RenderSettings.skybox = skyBoxMaterialToLoad;
        }
    }

    /// <summary>
    /// Change the render setting to set the light reflection intensity. 
    /// </summary>
    
    public void changeLightIntensity()
    {
        if (skyBoxMaterialToLoad != null)
        {
            RenderSettings.reflectionIntensity = skyBoxLightIntensity;
        }
    }
}