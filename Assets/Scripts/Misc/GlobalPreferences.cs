using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


/// <summary>
/// Stores calibration data for trial use in a single place.
/// </summary>
public class GlobalPreferences : MonoBehaviour 
{

    // The single instance of this class
    public static GlobalPreferences Instance;

    // participant ID to differentiate data files
    public string participantID { get; private set; } = "Default";
    public void SetParticipantID(string _value)
    {
        participantID = _value == "" ? "Default" : _value;
    }

    // The number of paddles that the player is using.
    public PaddleChoice paddleChoice { get; private set; }
    public void SetPaddleChoice(PaddleChoice _value) { paddleChoice = _value; }

    // Target Line Height
    public TargetEnum.Height targetBaseHeight { get; private set; }
    public void SetTargetBaseHeight(TargetEnum.Height _value) { targetBaseHeight = _value; }

    // The current session
    public SessionType.Session session { get; private set; }
    public void SetSession(SessionType.Session _value) { session = _value; }

    // Test period of this instance
    public DifficultyChoice startingDifficulty { get; private set; }
    public void SetStartingDifficulty(DifficultyChoice _value) { startingDifficulty = _value; }

    // Music for the scene (-1 on for the -select music- element in list)
    public int sceneMusicIndex { get; private set; }
    public void SetMusic(int _value) { sceneMusicIndex = _value-1; }
    //all music audio clip 
    public List<AudioClip> SceneMusic = new List<AudioClip>();

    //toggle on off ambiant sound 
    public bool toggleAmbiantSoundOn { get; private set; }
    public void SetToggleAmbiantSoundOn(bool _value) { toggleAmbiantSoundOn = _value; }

    //Volume of the audio sources in the scene
    public int StartingMusicVolume { get; private set; }
    public void SetStartingMusicVolume(int _value) { StartingMusicVolume = _value; }
    

    // Time limit for practise condition
    public int practiseMaxTrialTime { get; private set; }
    public void SetPractiseMaxTrialTime(int _value) { practiseMaxTrialTime = _value; }

    // Time per level for showcase condition
    public int showcaseTimePerCondition { get; private set; }
    public void SetShowcaseTimePerCondition(int _value) { showcaseTimePerCondition = _value; }

    public float timeConversionToMinute { get; private set; } = 60f;

    // value affecting various metrics increasing randomness and general difficulty
    public int practiseStartingLevel { get; private set; }
    public void SetPractiseStartingLevel(int _value) { practiseStartingLevel = _value; }

    // Duration for which ball should be held before dropping upon reset
    public int ballResetHoverSeconds { get; private set; }
    public void SetBallResetHoverSeconds(int _value) { ballResetHoverSeconds = _value; }

    // Play video at the start
    public bool playVideo { get; private set; }
    public void SetPlayVideo(bool _value) { playVideo = _value; }

    // Selected environment
    public int environmentIndex { get; private set; }
    public void SetEnvironmentIndex(int _value) { environmentIndex = _value; }

    // all environment prefabs
    public List<EnvironmentOptions> environments = new List<EnvironmentOptions>();
   
    //Starting wind level
    public int StartingWindLevel { get; private set; }
    public void SetStartingWindLevel(int _value) { StartingWindLevel = _value; }

    // Distraction level (DL) of the level
    public DistractionLevel StartingDistractionLevel { get; private set; }
    public void SetStartingDistractionLevel(DistractionLevel _value) { StartingDistractionLevel = _value; }

    //Number of bounce to trigger the events according to the level of difficulty(base, moderate , maximal)
    public int[] bouncePerDiffLevelToTriggerEvents = new int[3];

    //toggle on off events on bounce 
    public bool toggleEventsOnBounce { get; private set; }
    public void SetToggleEventsOnBounce(bool _value) { toggleEventsOnBounce = _value; }


    /// <summary>
    /// Assign instance to this, or destroy it if Instance already exits and is not this instance.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
