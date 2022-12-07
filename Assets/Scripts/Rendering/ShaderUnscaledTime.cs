using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to allow animated shader to works even when timescale in game is set to 0 (game pause)
/// Replace default _Time variable in shader by a custom time variable  
/// </summary>
public class ShaderUnscaledTime : MonoBehaviour
{
    //Variable for the material and the renderer 
    private Material _mat;
    private Renderer _rend;

    //Will be incremented to serve as a new "time" variable
    private float new_Time = 0;

    //Increment of the new_time variable 
    public float customTimeIncrements = 0.03f;

    /// <summary>
    /// ///Populate material and renderer component variable with the current ones
    /// </summary>
    void Start()
    {
        _rend = this.GetComponent<Renderer>();
        _mat =_rend.material;
    }

   /// <summary>
   /// Increment the custom time variable every frame and pass that value to the shader attached to the objet
   /// </summary>
    void Update()
    {
        new_Time += customTimeIncrements;
        Shader.SetGlobalFloat("_CustomTime", new_Time );
    }
}
