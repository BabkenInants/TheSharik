using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;
    [SerializeField] private Color[] Themes;
    [SerializeField] private Image[] UIImages;
    [Range(1, 20)]
    public int themeScore = 5;
    public Color CurrentTheme;

    private int index;
    private int maxIndex;
    private Color bgColor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        index = 0;
        maxIndex = Themes.Length - 1;
        CurrentTheme = Themes[index];
    }
    
    private void OnEnable() => AllEvents.OnCoinCollected += CheckScore;
    private void OnDisable() => AllEvents.OnCoinCollected -= CheckScore;

    private void CheckScore()
    {
        if (Player.Instance.Score > 0 && Player.Instance.Score % themeScore == 0)
            NextTheme();
    }

    private void NextTheme()
    {
        index += index == maxIndex? -maxIndex : 1;
        bgColor = Themes[index];
        CurrentTheme = Themes[index];
        foreach (Image img in UIImages)
            img.DOColor(bgColor, 2f);
    }
}