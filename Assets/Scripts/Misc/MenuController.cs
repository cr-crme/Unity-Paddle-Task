using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;




public enum DistractionLevel {
    low, 
    mid,
    high
}


[Serializable]
public class EnvironmentOptions {
    [SerializeField] public  GameObject _environment;
    [SerializeField] public List<DistractionLevel> _distractionLevel;
    [SerializeField] public bool _asWind; 

    public List<string> GetNameOfDistractionLevel()
    {
         List<string> mylist = new List<string>();
        if (_distractionLevel.Count != 0)
        {
            for (int i = 0; i < _distractionLevel.Count; i++)
            {
                mylist.Add(_distractionLevel[i].ToString());
            }
        }

        return mylist;
    }
}


/// <summary>
/// Holds functions for responding to and recording preferences on menu.
/// </summary>
public class MenuController : MonoBehaviour {

    private GlobalPreferences globalControl;
    [SerializeField] private GameObject practiseCanvas;
    [SerializeField] private GameObject showcaseCanvas;
    [SerializeField] private GameObject tutorialCanvas;

    // Loads all saved preferences to the main menu
    private delegate void LoadCallback(bool resetToDefault);
    // Default value / callback for Preference loading / Action to setting
    Dictionary<string, (object, LoadCallback)> preferenceList;

    #region Initialization
    /// <summary>
    /// Disable VR for menu scene and hide warning text until needed.
    /// </summary>
    void Start()
    {
        globalControl = GlobalPreferences.Instance;
        populateMusicDropdown(music, globalControl.SceneMusic);

        preferenceList = new Dictionary<string, (object, LoadCallback)>(){
            { "paddle_choice", (GetPaddleChoice(), LoadPaddleChoiceToMenu) },
            { "environment", (GetEnvironment(), LoadEnvironmentToMenu) },
            { "session", (GetRecordSession(), LoadSessionToMenu) },
            { "practise_totalTime", (GetPractiseTotalTime(), LoadPractiseTotalTimeToMenu) },
            { "showcase_timePerTrial", (GetShowcaseTimePerTrial(), LoadShowcaseTimePerTrialToMenu) },
            { "difficulty", (GetDifficulty(), LoadDifficulty) },
            { "practise_level", (GetPractiseStartingLevel(), LoadPractiseStartingLevelToMenu) },
            { "targetHeight", (GetTargetHeight(), LoadTargetHeightToMenu) },
            { "hoverTime", (GetBallHoverTime(), LoadBallHoverTimeToMenu) },
            {"distraction", (GetDistractionLevel(), LoadDistractionLevel) },
            { "wind_level", (GetStartingWindLevel(), LoadStartingWindLevelToMenu) },
            { "music", (GetMusic(), LoadMusicToMenu) },
            { "toggleSounds", (GetToggleSounds(),  LoadToggleSounds) },
            { "musicVolume", (GetStartingMusicVolume(), LoadStartingMusicVolumeToMenu) },
            {"toggleEvents", (GetToggleEvents(),  LoadToggleEvents) }
        };

        // disable VR settings for menu scene
        UnityEngine.XR.XRSettings.enabled = false;

        // Load saved preferences
        LoadAllPreferences(false);
        UpdateConditionalUIObjects();

        
        showHideVolumeAndPreview();

    }
    public void LoadAllPreferences(bool resetToDefault)
    {
        foreach (KeyValuePair<string, (object, LoadCallback)> callback in preferenceList)
        {
            callback.Value.Item2(resetToDefault);  // Call the Load callback
        }
    }
    // Clears all saved main menu preferences
    public void ResetPlayerPrefsToDefault()
    {
        PlayerPrefs.DeleteAll();
        LoadAllPreferences(true);
    }

    void UpdateConditionalUIObjects()
    {
        ShowProperSessionCanvas();
    }
    #endregion

    #region Finalization
    public void NextScene()
    {

        previewButton.GetComponentInChildren<AudioSource>().Stop();

        //Here first load the paddle scene then load the prefab of the environment
        SceneManager.LoadScene("Paddle");
    }

    /// <summary>
    /// Re-enable VR when this script is disabled (since it is disabled on moving into next scene).
    /// </summary>
    void OnDisable()
    {
        UnityEngine.XR.XRSettings.enabled = true;
    }
    #endregion

    #region GenericInformation
    [SerializeField] private TMP_Dropdown paddlesChoice;
    [SerializeField] private TMP_Dropdown environment;
    [SerializeField] private TMP_Dropdown distractionLevel;

    /// <summary>
    /// Records an alphanumeric participant ID. Hit enter to record. May be entered multiple times
    /// but only last submission is used. Called using a dynamic function in the inspector
    /// of the textfield object.
    /// </summary>
    /// <param name="arg0"></param>
    public void RecordID(string arg0)
    {
        globalControl.SetParticipantID(arg0);
    }

    public void RecordPaddleChoice(int _value)
    {
        globalControl.SetPaddleChoice((PaddleChoice)_value);
        SavePaddleChoice(_value);
    }
    private void SetPaddleChoice(int _value)
    {
        paddlesChoice.value = _value;
    }
    private int GetPaddleChoice()
    {
        return paddlesChoice.value;
    }
    private void SavePaddleChoice(int _value)
    {
        PlayerPrefs.SetInt("paddle_choice", _value);
        PlayerPrefs.Save();
    }
    private void LoadPaddleChoiceToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["paddle_choice"].Item1;
        else if (PlayerPrefs.HasKey("paddle_choice"))
            _value = PlayerPrefs.GetInt("paddle_choice");
        else
            return;

        RecordPaddleChoice(_value);
        SetPaddleChoice(_value);
    }



    public void RecordEnvironment(int _value)
    {
        //to set the possible level of distraction of the scene corresponding to specific environment
        List<string> nameOfDistractionLevel = GlobalPreferences.Instance.environments[_value].GetNameOfDistractionLevel();
        populateDistractionLevelDropdown(distractionLevel, nameOfDistractionLevel);
        RecordDistractionLevel(distractionLevel.value);

        globalControl.SetEnvironmentIndex(_value);
        SaveEnvironment(_value);

        if (GlobalPreferences.Instance.environments[_value]._asWind != true)
        {
            startingWindLevel.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            startingWindLevel.transform.parent.gameObject.SetActive(true);
        }


    }
    private void SetEnvironment(int _value)
    {
        environment.value = _value;
    }
    private int GetEnvironment()
    {
        return environment.value;
    }
    private void SaveEnvironment(int _value)
    {
        PlayerPrefs.SetInt("environment", _value);
        PlayerPrefs.Save();
    }
    private void LoadEnvironmentToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["environment"].Item1;
        else if (PlayerPrefs.HasKey("environment"))
            _value = PlayerPrefs.GetInt("environment");
        else
            return;

        RecordEnvironment(_value);
        SetEnvironment(_value);
    }
    #endregion

    #region SessionType
    [SerializeField] private TMP_Dropdown session;
    // Records the Session from the dropdown menu
    public void RecordSession(int _value)
    {
        globalControl.SetSession((SessionType.Session)_value);
        SaveSession(_value);
        UpdateConditionalUIObjects();
    }
    private void SetRecordSession(int _value)
    {
        session.value = _value;
    }
    private int GetRecordSession()
    {
        return session.value;
    }
    public void SaveSession(int menuInt)
    {
        PlayerPrefs.SetInt("session", menuInt);
        PlayerPrefs.Save();
    }
    private void LoadSessionToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["session"].Item1;
        else if (PlayerPrefs.HasKey("session"))
            _value = PlayerPrefs.GetInt("session");
        else
            return;

        RecordSession(_value);
        SetRecordSession(_value);
    }
    #endregion

    #region TrialTime
    [SerializeField] private Slider practiseTotalTime;
    [SerializeField] private TextMeshProUGUI practiseTotalTimeText;
    [SerializeField] private Slider showcaseTimePerTrial;
    [SerializeField] private TextMeshProUGUI showcaseTimePerTrialText;
    public void RecordPractiseTotalTime(float _value)
    {
        globalControl.SetPractiseMaxTrialTime((int)_value);
        UpdatePractiseTotalTimeText((int)_value);
        SavePractiseTotalTime((int)_value);
    }
    private void UpdatePractiseTotalTimeText(int _value)
    {
        if (_value != 0)
        {
            practiseTotalTimeText.text = $"Time: {_value} minutes";
        }
        else
        {
            practiseTotalTimeText.text = $"Time: No Limit";
        }
    }
    private void SetPractiseTotalTime(int _value)
    {
        practiseTotalTime.value = _value;
    }
    private int GetPractiseTotalTime()
    {
        return (int)practiseTotalTime.value;
    }
    private void SavePractiseTotalTime(int _value)
    {
        PlayerPrefs.SetInt("practise_totalTime", _value);
        PlayerPrefs.Save();
    }
    private void LoadPractiseTotalTimeToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["practise_totalTime"].Item1;
        else if (PlayerPrefs.HasKey("practise_totalTime"))
            _value = PlayerPrefs.GetInt("practise_totalTime");
        else
            return;

        RecordPractiseTotalTime(_value);
        SetPractiseTotalTime(_value);
    }

    public void RecordShowcaseTimePerTrial(float _value)
    {
        globalControl.SetShowcaseTimePerCondition((int)_value);
        UpdateShowcaseTimePerTrialText((int)_value);
        SaveShowcaseTimePerTrial((int)_value);
    }
    private void UpdateShowcaseTimePerTrialText(int _value)
    {
        showcaseTimePerTrialText.text = $"Time: {_value} minutes";
    }
    private void SetShowcaseTimePerTrial(int _value)
    {
        showcaseTimePerTrial.value = _value;
    }
    private int GetShowcaseTimePerTrial()
    {
        return (int)showcaseTimePerTrial.value;
    }
    private void SaveShowcaseTimePerTrial(int _value)
    {
        PlayerPrefs.SetInt("showcase_timePerTrial", _value);
        PlayerPrefs.Save();
    }
    private void LoadShowcaseTimePerTrialToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["showcase_timePerTrial"].Item1;
        else if (PlayerPrefs.HasKey("showcase_timePerTrial"))
            _value = PlayerPrefs.GetInt("showcase_timePerTrial");
        else
            return;

        RecordShowcaseTimePerTrial(_value);
        SetShowcaseTimePerTrial(_value);
    }

    private void ShowProperSessionCanvas()
    {
        if (globalControl.session == SessionType.Session.PRACTISE)
        {
            practiseCanvas.SetActive(true);
            showcaseCanvas.SetActive(false);
            tutorialCanvas.SetActive(false);
        }
        else if (globalControl.session == SessionType.Session.SHOWCASE)
        {
            practiseCanvas.SetActive(false);
            showcaseCanvas.SetActive(true);
            tutorialCanvas.SetActive(false);
        }
        else if (globalControl.session == SessionType.Session.TUTORIAL)
        {
            showcaseCanvas.SetActive(false);
            practiseCanvas.SetActive(false);
            tutorialCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Not implemented Session");
        }
    }
    #endregion


    #region Difficulty
    [SerializeField] private TMP_Dropdown difficulty;
    public void RecordDifficulty(int _value)
    {
        globalControl.SetStartingDifficulty((DifficultyChoice)_value);
        SaveDifficulty(_value);
    }
    private void SetDifficulty(int _value)
    {
        difficulty.value = _value;
    }
    private int GetDifficulty()
    {
        return difficulty.value;
    }
    private void SaveDifficulty(int _value)
    {
        PlayerPrefs.SetInt("difficulty", _value);
        PlayerPrefs.Save();
    }
    private void LoadDifficulty(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["difficulty"].Item1;
        else if (PlayerPrefs.HasKey("difficulty"))
            _value = PlayerPrefs.GetInt("difficulty");
        else
            return;

        RecordDifficulty(_value);
        SetDifficulty(_value);
    }
    #endregion


    #region Distraction
 

    public void populateDistractionLevelDropdown(TMP_Dropdown dropdown, List<string> newDropdownElements)
    {
        List<string> list = new List<string>();

        for (int i = 0; i< newDropdownElements.Count; i++)
        {
            list.Add(newDropdownElements[i]);
        }
        
        dropdown.ClearOptions();
        dropdown.AddOptions(list);
        dropdown.value = list.Count;
    }

    public void RecordDistractionLevel(int _value)
    {
        globalControl.SetStartingDistractionLevel((DistractionLevel)_value);
        SaveDistractionLevel(_value);
       
        //to ajust wind slider to match default setting of distraction level
        switch (distractionLevel.options[distractionLevel.value].text)
        {
            case "low":
                startingWindLevel.value = 1;
                RecordStartingWindLevel(1);
                break;

            case "mid":
                startingWindLevel.value = 2;
                RecordStartingWindLevel(2);
                break;

            case "high":
                startingWindLevel.value = 3;
                RecordStartingWindLevel(3);
                break;
        }  
    }
       

    
    private void SetDistractionLevel(int _value)
    {
            distractionLevel.value = _value;
    }
    private int GetDistractionLevel()
    {
        return distractionLevel.value;
    }
    private void SaveDistractionLevel(int _value)
    {
        PlayerPrefs.SetInt("distraction", _value);
        PlayerPrefs.Save();
    }
    private void LoadDistractionLevel(bool resetToDefault)
    {
        int _value;

        if (resetToDefault)
            _value = (int)preferenceList["distraction"].Item1;
        
        else if (PlayerPrefs.HasKey("distraction"))
            _value = PlayerPrefs.GetInt("distraction");

        else
            return;

        RecordDistractionLevel(_value);
        SetDistractionLevel(_value);
    }



    //Toggle on/off Events triggered by number of bounce
    [SerializeField] public Toggle toggleEvents;

    public void updateTriggerEventOn()
    {
        RecordToggleEvents(GetToggleEvents());
    }

    public void RecordToggleEvents(bool _value)
    {
        globalControl.SetToggleEventsOnBounce(_value);
       
        SaveToggleEvents(_value);
        
    }

    private void SetToggleEvents(bool _value)
    {
        toggleEvents.isOn = _value;
    }
    private bool GetToggleEvents()
    {
        return toggleEvents.isOn;
    }
    private void SaveToggleEvents(bool _value)
    {

        PlayerPrefs.SetInt("toggleEvents", _value ? 1 : 0);
        PlayerPrefs.Save();
    }
    private void LoadToggleEvents(bool resetToDefault)
    {
        bool _value;
        if (resetToDefault)
            _value = (int)preferenceList["toggleEvents"].Item1 == 1;
        else if (PlayerPrefs.HasKey("toggleEvents"))
            _value = PlayerPrefs.GetInt("toggleEvents") == 1;
        else
            return;

        RecordToggleEvents(_value);
        SetToggleEvents(_value);

    }

    #endregion

    #region Level
    [SerializeField] private Slider practiseStartingLevel;
    [SerializeField] private TextMeshProUGUI practiseStartingLevelText;
    public void RecordPractiseStartingLevel(float _value)
    {
        globalControl.SetPractiseStartingLevel((int)_value);
        UpdatePractiseStartingLevelText((int)_value);
        SavePractiseStartingLevel((int)_value);
    }
    private void UpdatePractiseStartingLevelText(int _value)
    {
        practiseStartingLevelText.text = $"Level {_value}";
    }
    private void SetPractiseStartingLevel(int _value)
    {
        practiseStartingLevel.value = _value;
    }
    private int GetPractiseStartingLevel()
    {
        return (int)practiseStartingLevel.value;
    }
    private void SavePractiseStartingLevel(int _value)
    {
        PlayerPrefs.SetInt("practise_level", _value);
        PlayerPrefs.Save();
    }
    private void LoadPractiseStartingLevelToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["practise_level"].Item1;
        else if (PlayerPrefs.HasKey("practise_level"))
            _value = PlayerPrefs.GetInt("practise_level");
        else
            return;

        RecordPractiseStartingLevel(_value);
        SetPractiseStartingLevel(_value);
    }
    #endregion

    #region Wind Level
    [SerializeField] private Slider startingWindLevel;
    [SerializeField] private TextMeshProUGUI startingWindLevelText;
    public void RecordStartingWindLevel(float _value)
    {
        
        globalControl.SetStartingWindLevel((int)_value);
        UpdateStartingWindLevelText((int)_value);
        SaveStartingWindLevel((int)_value);
    }
    private void UpdateStartingWindLevelText(int _value)
    {
        string windText = "";
        
        switch (_value)
        {
            case 0:
                windText = "no wind";
                break;
            case 1:
                windText = "low";
                break;

            case 2:
                windText = "mid";
                break;

            case 3:
                windText = "high";
                break;
        }

        startingWindLevelText.text = $"Wind Level: {windText}";
    }
    private void SetStartingWindLevel(int _value)
    {
        startingWindLevel.value = _value;
    }
    private int GetStartingWindLevel()
    {
        return (int)startingWindLevel.value;
    }
    private void SaveStartingWindLevel(int _value)
    {
        PlayerPrefs.SetInt("wind_level", _value);
        PlayerPrefs.Save();
    }
    private void LoadStartingWindLevelToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["wind_level"].Item1;
        else if (PlayerPrefs.HasKey("wind_level"))
            _value = PlayerPrefs.GetInt("wind_level");
        else
            return;

        RecordStartingWindLevel(_value);
        SetStartingWindLevel(_value);
    }
    #endregion

    #region Music

    //Manage the music dropdown
    [SerializeField] private TMP_Dropdown music;
   
    public void RecordMusic(int _value)
    {
        globalControl.SetMusic(_value);
        SaveMusic(_value);

        //preview music button and vilume slider
        showHideVolumeAndPreview();



    }

    private void showHideVolumeAndPreview()
    {
        if (music.value == 0)
        {
            previewButton.GetComponentInChildren<AudioSource>().Stop();
            startingMusicVolume.transform.parent.gameObject.SetActive(false);
            previewButton.transform.parent.gameObject.SetActive(false);
        }

        else
        {
            startingMusicVolume.transform.parent.gameObject.SetActive(true);
            previewButton.transform.parent.gameObject.SetActive(true);
            previewButton.GetComponentInChildren<AudioSource>().Stop();

            if (globalControl.sceneMusicIndex > 0)
            {
                previewButton.GetComponentInChildren<AudioSource>().clip = globalControl.SceneMusic[globalControl.sceneMusicIndex];
            }
            updatePreviewButtonText();
        }

    }

        private void SetMusic(int _value)
    {
        music.value = _value;
    }
    private int GetMusic()
    {
        return music.value;
    }
    private void SaveMusic(int _value)
    {
        PlayerPrefs.SetInt("music", _value);
        PlayerPrefs.Save();
    }
    private void LoadMusicToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["music"].Item1;
        else if (PlayerPrefs.HasKey("music"))
            _value = PlayerPrefs.GetInt("music");
        else
            return;

        RecordMusic(_value);
        SetMusic(_value);

    }

    public void populateMusicDropdown(TMP_Dropdown dropdown, List<AudioClip> DropdownElements)
    {
        List<string> list = new List<string>() {" - No Music - "};

        for (int i = 0; i < DropdownElements.Count; i++)
        {
            list.Add(DropdownElements[i].name);
        }


        dropdown.ClearOptions();
        dropdown.AddOptions(list);
    }


    // Preview music 

    [SerializeField] private Button previewButton;
    public void updatePreviewButtonText() 
    {
       
        string text;
        if (music.value != 0 && music.enabled != false )
        {
            previewButton.enabled = true;
            if (GetComponentInChildren<AudioSource>().isPlaying == false)
            {
                text = "Preview Music";
            }
            else
            {
                text = "stop";
            }
            previewButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
       
       
    }

    public void PlayPreviewMusic()
    {
        if (globalControl.sceneMusicIndex >= 0)
        {
            if (GetComponentInChildren<AudioSource>().isPlaying == false)
            {
                previewButton.GetComponentInChildren<AudioSource>().enabled = true;

                previewButton.GetComponentInChildren<AudioSource>().clip = globalControl.SceneMusic[globalControl.sceneMusicIndex];
                previewButton.GetComponentInChildren<AudioSource>().Play();
            }

            else
            {
                previewButton.GetComponentInChildren<AudioSource>().enabled = false;
            }
        }
        updatePreviewButtonText();
    }

    [SerializeField] public Toggle toggleSounds;

    public void RecordToggleSounds(bool _value)
    {
        //print(_value);
        globalControl.SetToggleAmbiantSoundOn(_value);
        SaveToggleSounds(_value);
    }

    private void SetToggleSounds(bool _value)
    {
        toggleSounds.isOn = _value;
    }
    private bool GetToggleSounds()
    {
        return toggleSounds.isOn;
    }
    private void SaveToggleSounds(bool _value)
    {
        
        PlayerPrefs.SetInt("toggleSounds", _value ? 1 : 0);
        PlayerPrefs.Save();
    }
    private void LoadToggleSounds(bool resetToDefault)
    {
        bool _value;
        if (resetToDefault)
            _value = (int)preferenceList["toggleSounds"].Item1 == 1;
        else if (PlayerPrefs.HasKey("toggleSounds"))
            _value = PlayerPrefs.GetInt("toggleSounds") == 1;
        else
            return;

        RecordToggleSounds(_value);
        SetToggleSounds(_value);

    }


    //Volume of audio in the scene (ambiant and music)
    [SerializeField] private Slider startingMusicVolume;
    [SerializeField] private TextMeshProUGUI startingMusicVolumeText;
    public void RecordStartingMusicVolume(float _value)
    {

        globalControl.SetStartingMusicVolume((int)_value);
        UpdateStartingMusicVolumeText((int)_value);
        SaveStartingMusicVolume((int)_value);

        previewButton.GetComponentInChildren<AudioSource>().volume = _value/10;
       
    }
    private void UpdateStartingMusicVolumeText(int _value)
    {
        startingMusicVolumeText.text = $"Volume: {_value}";
    }
    private void SetStartingMusicVolume(int _value)
    {
        startingMusicVolume.value = _value;
    }
    private int GetStartingMusicVolume()
    {
        return (int)startingMusicVolume.value;
    }
    private void SaveStartingMusicVolume(int _value)
    {
        PlayerPrefs.SetInt("musicVolume", _value);
        PlayerPrefs.Save();
    }
    private void LoadStartingMusicVolumeToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["musicVolume"].Item1;
        else if (PlayerPrefs.HasKey("musicVolume"))
            _value = PlayerPrefs.GetInt("musicVolume");
        else
            return;

        RecordStartingMusicVolume(_value);
        SetStartingMusicVolume(_value);
    }
  


#endregion

    #region Target
    [SerializeField] private TMP_Dropdown targetHeight;
    public void RecordTargetHeight(int _value)
    {
        globalControl.SetTargetBaseHeight((TargetEnum.Height)_value);
        SaveTargetHeight(_value);
    }
    private void SetTargetHeight(int _value)
    {
        targetHeight.value = _value;
    }
    private int GetTargetHeight()
    {
        return targetHeight.value;
    }
    private void SaveTargetHeight(int _value)
    {
        PlayerPrefs.SetInt("targetHeight", _value);
        PlayerPrefs.Save();
    }
    private void LoadTargetHeightToMenu(bool resetToDefault)
    {
        int _value;
        if (resetToDefault)
            _value = (int)preferenceList["targetHeight"].Item1;
        else if (PlayerPrefs.HasKey("targetHeight"))
            _value = PlayerPrefs.GetInt("targetHeight");
        else
            return;

        RecordTargetHeight(_value);
        SetTargetHeight(_value);
    }
    #endregion

    #region Ball
    [SerializeField] private Slider ballHoverTime;
    [SerializeField] private TextMeshProUGUI ballHoverTimeText;
   
    public void RecordBallHoverTime(float _value)
    {
        globalControl.SetBallResetHoverSeconds((int)_value);
        UpdateBallHoverTimeText(_value);
        SaveBallHoverTime(_value);
    }
    private void UpdateBallHoverTimeText(float _value)
    {
        ballHoverTimeText.text = $"{_value} seconds";
    }
    private void SetBallHoverTime(float _value)
    {
        ballHoverTime.value = _value;
    }
    private float GetBallHoverTime()
    {
        return ballHoverTime.value;
    }
    private void SaveBallHoverTime(float _value)
    {
        PlayerPrefs.SetFloat("hoverTime", _value);
        PlayerPrefs.Save();
    }
    private void LoadBallHoverTimeToMenu(bool resetToDefault)
    {
        float _value;
        if (resetToDefault)
            _value = (float)preferenceList["hoverTime"].Item1;
        else if (PlayerPrefs.HasKey("hoverTime"))
            _value = PlayerPrefs.GetFloat("hoverTime");
        else
            return;

        RecordBallHoverTime(_value);
        SetBallHoverTime(_value);
    }
    #endregion



  

    
    
}
