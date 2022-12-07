using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible of getting all objets with distraction level tags (DL_moderate, DL_Maximal) and hiding them
/// For low distraction, objects with no tags are rendered
/// </summary>
public class DistractionManager : MonoBehaviour
{
    private DistractionLevel distractionLevel;
    private List<string> tagsToRemove = new List<string>();
    private GameObject[] objectsToRemove;
    private string _tag;


    /// <summary>
    /// Get the distraction level from menu selection
    /// call method to create a list of tags corresponding to the chosen distraction level
    /// call method to set all object with given tag to be inactive 
    /// </summary>
    private void Awake()
    {
        distractionLevel = GlobalPreferences.Instance.StartingDistractionLevel;

        tagsToRemove = getListofTags(distractionLevel);

        if (tagsToRemove.Count != 0)
        {
            hideObjectsWithTags(tagsToRemove);
        }
    }


    /// <summary>
    /// Method that create a list of tags corresponding to the chosen level of distraction 
    /// </summary>
    private List<string> getListofTags(DistractionLevel distractionLevel)
    { 
        switch (distractionLevel)
        {
            case DistractionLevel.low:
                tagsToRemove.Add("DistractionLevel_Mid");
                tagsToRemove.Add("DistractionLevel_High");
        break;

            case DistractionLevel.mid:
                tagsToRemove.Add("DistractionLevel_High");
                break;

            case DistractionLevel.high:
                break;
        }
        return tagsToRemove;
    }


    /// <summary>
    /// Method that set inactive all gameObject with the corresponding tag
    /// </summary>
    private void hideObjectsWithTags(List<string> tagsToRemove )
    {
            for (int i = 0; i < tagsToRemove.Count; i++)
            {
                _tag = tagsToRemove[i];
                objectsToRemove = GameObject.FindGameObjectsWithTag(_tag);

                if (objectsToRemove.Length != 0)
                {
                    for (int j = 0; j < objectsToRemove.Length; j++)
                    {
                       objectsToRemove[j].SetActive(false);
                    }
                }
            }
    }





}
