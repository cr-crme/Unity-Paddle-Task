using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using System.Linq;


/// <summary>
/// Class to store gameObject holding a script to trigger when specific number of bounces is reached. 
/// </summary>
[Serializable]
public class EventsOnBallBounce
{ 
     public GameObject _event;
}


/// <summary>
/// Class that calls methods when a specific number of ball bounces is reached 
/// </summary>
public class EventManager : MonoBehaviour
{

    private bool triggerEventOn;

    //List of event to call 
    public List<EventsOnBallBounce> triggerableEvents = new List<EventsOnBallBounce>();

   
    //List of class holding the methods to be called 
    private List<MonoBehaviour> classesOfTheMethods = new List<MonoBehaviour>();


    private List<MethodInfo> methods = new List<MethodInfo>();


    /// <summary>
    /// On start create a list of the methods to be triggered
    /// </summary>
    private void Awake()
    {
        triggerEventOn = GlobalPreferences.Instance.toggleEventsOnBounce;

        if (triggerEventOn == true)
        {
            if (triggerableEvents.Count > 0)
            {
                populateMethodsList();
            }
        }
    }


    /// <summary>
    /// Method called to trigger an event at random
    /// </summary>
    public void triggerEvent()
    {
        if (triggerEventOn == true)
        {
            if (triggerableEvents.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, methods.Count);

                methods[randomIndex].Invoke(classesOfTheMethods[randomIndex], null);
            }
            else
            {
                Debug.Log("OnBounce events: no event to trigger");
            }
        }
    }



    /// <summary>
    /// Populate the list of methods to trigger using the flags
    /// Method native to unreal not added to the list. 
    /// </summary>
    private void populateMethodsList()
    {
        IEnumerable<MethodInfo> classMethods;

        //the flags used to identify which method to get  
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default;
        methods.Clear();
        classesOfTheMethods.Clear();
        //populate a list holding all the class in which we'll get the methods
        for (int i = 0; i < triggerableEvents.Count; i++)
        {
                var monoBehaviours = triggerableEvents[i]._event.GetComponents<MonoBehaviour>();
                foreach (var monoBehaviour in monoBehaviours)
                {
                if (monoBehaviour != null)
                    {
                        Type currentType = monoBehaviour.GetType();
                        classMethods = currentType.GetMethods(flags)
                        .Where(x => x.DeclaringType == currentType);

                        methods.AddRange(classMethods);
                        classesOfTheMethods.Add(monoBehaviour);
                    }
                }
        } 
    }
}



