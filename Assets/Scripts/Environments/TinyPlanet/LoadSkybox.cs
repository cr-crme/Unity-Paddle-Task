using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for setting a Skybox on load
/// </summary>
public class LoadSkybox : MonoBehaviour
{
   /// The skyBox Material to be set on load
   [SerializeField]
   private Material skyBoxMaterialToLoad;


    /// <summary>
    /// Call the method responsible for setting the skybox material. 
    /// </summary>
    private void Awake()
    {
        changeSkyBox();
    }


    /// <summary>
    /// Change the render setting to set the skybox material. 
    /// </summary>
    private void changeSkyBox()
    {
        if (skyBoxMaterialToLoad !=null)
        {
            RenderSettings.skybox = skyBoxMaterialToLoad;
        }
    }
}
