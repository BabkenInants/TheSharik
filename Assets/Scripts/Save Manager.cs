using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [HideInInspector] public int bestScore;
    [HideInInspector] public bool completedTutorial;
    [HideInInspector] public bool musicIsOn = true;
    [HideInInspector] public bool sfxIsOn = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Load();
    }

    private void OnEnable() => AllEvents.EndGame += SaveData;

    private void OnDisable() => AllEvents.EndGame -= SaveData;

    public void Load()
    {
        bestScore = PlayerPrefs.GetInt("bestScore");
        completedTutorial = PlayerPrefs.GetInt("completedTutorial") == 1;
        if (PlayerPrefs.HasKey("musicIsOn")) musicIsOn = PlayerPrefs.GetInt("musicIsOn") == 1;
        else musicIsOn = true;
        if (PlayerPrefs.HasKey("sfxIsOn")) sfxIsOn = PlayerPrefs.GetInt("sfxIsOn") == 1;
        else sfxIsOn = true;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("bestScore", bestScore);
        PlayerPrefs.SetInt("completedTutorial", completedTutorial? 1 : 0);
        PlayerPrefs.SetInt("musicIsOn", musicIsOn? 1 : 0);
        PlayerPrefs.SetInt("sfxIsOn", sfxIsOn? 1 : 0);
    }
}