using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip music;
    public AudioClip buttonSFX;
    public AudioClip coinPickupSFX;
    public AudioClip obstacleHitSFX;
    private AudioSource musicAudioSource;
    public bool musicIsOn;
    public bool sfxIsOn;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() => Subscribe();

    private void OnDisable() => Unsubscribe();

    private void Start()
    {
        musicIsOn = SaveManager.Instance.musicIsOn;
        sfxIsOn = SaveManager.Instance.sfxIsOn;
        GameObject temp = new GameObject();
        temp.transform.parent = transform;
        temp.name = "MusicAudioSource";
        musicAudioSource = temp.AddComponent<AudioSource>();
        musicAudioSource.Stop();
        musicAudioSource.clip = music;
        musicAudioSource.loop = true;
        if (musicIsOn)
            musicAudioSource.Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (!sfxIsOn) return;
        GameObject temp = new GameObject();
        temp.transform.parent = transform;
        temp.name = clip.name + "SFX";
        AudioSource tempSource = temp.AddComponent<AudioSource>();
        temp.AddComponent<Killer>().lifeTime = clip.length + .2f;
        tempSource.clip = clip;
        tempSource.loop = false;
        tempSource.Play();
    }

    private void Subscribe()
    {
        AllEvents.OnMusicButtonPressed += ChangeMusicState;
        AllEvents.OnSFXButtonPressed += ChangeSFXState;
        AllEvents.OnPlayButtonPressed += PlayButtonSFX;
        AllEvents.OnCoinCollected += PlayCoinSFX;
        AllEvents.OnObstacleHit += PlayObstacleSFX;
    }
    
    private void Unsubscribe()
    {
        AllEvents.OnMusicButtonPressed -= ChangeMusicState;
        AllEvents.OnSFXButtonPressed -= ChangeSFXState;
        AllEvents.OnPlayButtonPressed -= PlayButtonSFX;
        AllEvents.OnCoinCollected -= PlayCoinSFX;
        AllEvents.OnObstacleHit -= PlayObstacleSFX;
    }
    
    private void ChangeMusicState()
    {
        musicIsOn = !musicIsOn;
        SaveManager.Instance.musicIsOn = music;
        musicAudioSource.mute = !musicIsOn;
        PlaySFX(buttonSFX);
    }

    private void ChangeSFXState()
    {
        if (sfxIsOn) PlaySFX(buttonSFX);
        sfxIsOn = !sfxIsOn;
        SaveManager.Instance.sfxIsOn = sfxIsOn;
        if (sfxIsOn) PlaySFX(buttonSFX);
    }

    private void PlayButtonSFX() => PlaySFX(buttonSFX);

    private void PlayCoinSFX() => PlaySFX(coinPickupSFX);

    private void PlayObstacleSFX() => PlaySFX(obstacleHitSFX);
}
