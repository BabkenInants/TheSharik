using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Sprite musicButtonEnabledSprite;
    [SerializeField] private Sprite musicButtonDisabledSprite;
    [SerializeField] private Image sfxButtonImage;
    [SerializeField] private Sprite sfxButtonEnabledSprite;
    [SerializeField] private Sprite sfxButtonDisabledSprite;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text lastScoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite heart;

    public void PlayButton() => AllEvents.RaiseOnPlayButtonPressed();

    public void MusicButton()
    {
        AllEvents.RaiseOnMusicButtonPressed();
        musicButtonImage.sprite = AudioManager.Instance.musicIsOn ? musicButtonEnabledSprite : musicButtonDisabledSprite;
    }

    public void SFXButton()
    {
        AllEvents.RaiseOnSFXButtonPressed();
        sfxButtonImage.sprite = AudioManager.Instance.sfxIsOn ? sfxButtonEnabledSprite : sfxButtonDisabledSprite;
    }

    private void Start()
    {
        musicButtonImage.sprite = SaveManager.Instance.musicIsOn ? musicButtonEnabledSprite : musicButtonDisabledSprite;
        sfxButtonImage.sprite = SaveManager.Instance.sfxIsOn ? sfxButtonEnabledSprite : sfxButtonDisabledSprite;
        scoreText.text = "0";
        lastScoreText.text = "Last Score: 0";
        UpdateBestScore();
    }

    private void OnEnable()
    {
        AllEvents.OnCoinCollected += UpdateScoreText;
        AllEvents.EndGame += UpdateBestScore;
        AllEvents.EndGame += UpdateLastScore;
        AllEvents.StartGame += UpdateScoreText;
        AllEvents.StartGame += UpdateHearts;
        AllEvents.OnObstacleHit += UpdateHearts;
    }

    private void OnDisable()
    {
        AllEvents.OnCoinCollected -= UpdateScoreText;
        AllEvents.EndGame -= UpdateLastScore;
        AllEvents.EndGame -= UpdateBestScore;
        AllEvents.StartGame -= UpdateScoreText;
        AllEvents.StartGame -= UpdateHearts;
        AllEvents.OnObstacleHit -= UpdateHearts;
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = i < Player.Instance.Health? heart : emptyHeart;
    }
    private void UpdateScoreText() => scoreText.text = Player.Instance.Score.ToString();
    private void UpdateLastScore() => lastScoreText.text = "Last Score: " + Player.Instance.Score;
    private void UpdateBestScore() => bestScoreText.text = "Best Score: " + SaveManager.Instance.bestScore;
}