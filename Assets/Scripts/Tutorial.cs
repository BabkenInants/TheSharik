using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
    public GameObject panel;
    public Text playBtnText;
    public Text tapText;
    public Text fiveFoldText;
    public Image playButton;
    public Condition condition;
    public Button[] buttons;
    private bool _pressedPlayBtn;
    private bool _pressedMBtn;
    private SaveManager _saves;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        AllEvents.OnPlayButtonPressed += PlayBtnT;
    }

    private void OnDisable()
    {
        AllEvents.OnPlayButtonPressed -= PlayBtnT;
    }

    private void Start() 
    {
        _saves = SaveManager.Instance;
        if (_saves.completedTutorial) Destroy(gameObject);
    }

    public enum Condition
    {
        PlayBtn, Tap, FiveFold, Completed
    }

    public void PlayBtnT() => _pressedPlayBtn = true;

    public void MainBtnT()
    {
        if(condition is Condition.Tap or Condition.FiveFold)
            _pressedMBtn = true;
    }
    
    public IEnumerator RunTutorial()
    {
        //Enabling(changing alfa chanels) tutorial panel
        condition = Condition.PlayBtn;
        panel.SetActive(true);
        panel.GetComponent<Image>().DOFade(.6f, .5f);
        playBtnText.DOFade(1, 1f);
        playButton.DOColor(Color.white, .5f);

        //Disabling all buttons except play button
        foreach (Button button in buttons)
            button.interactable = false;

        yield return new WaitForSeconds(1f);

        //Waiting for player to press the play button
        while (!_pressedPlayBtn) yield return null;

        //Enabling disabled buttons back so after game restart we'd be able to press them again
        foreach (Button button in buttons)
            button.interactable = true;

        //Explaining next step
        panel.GetComponent<Image>().DOFade(.4f, .25f);
        playBtnText.DOFade(0, .25f);
        yield return new WaitForSeconds(.25f);
        panel.GetComponent<Image>().DOFade(.6f, .25f);
        playButton.DOColor(ThemeManager.Instance.CurrentTheme, .5f);
        tapText.DOFade(1, .5f);
        yield return new WaitForSeconds(.5f);
        condition = Condition.Tap;

        //Waiting for player to tap on screen to show next step
        while (!_pressedMBtn)
            yield return new WaitForSeconds(.1f);

        //Showing next step
        panel.GetComponent<Image>().DOFade(.4f, .25f);
        tapText.DOFade(0, .25f);
        yield return new WaitForSeconds(.25f);
        panel.GetComponent<Image>().DOFade(.6f, .25f);
        fiveFoldText.DOFade(1f, .5f);
        yield return new WaitForSeconds(.5f);
        condition = Condition.FiveFold;
        _pressedMBtn = false;

        //Waiting for player to tap again to show the last step
        while (!_pressedMBtn)
            yield return new WaitForSeconds(.1f);

        //Disabling the panel and saving the game so the tutorial won't appear on next game start
        panel.GetComponent<Image>().DOFade(0, .5f);
        fiveFoldText.DOFade(0, .5f);
        condition = Condition.Completed;
        yield return new WaitForSeconds(.5f);
        panel.SetActive(false);
        _saves.completedTutorial = true;
        _saves.SaveData();
        Destroy(gameObject);
    }
}