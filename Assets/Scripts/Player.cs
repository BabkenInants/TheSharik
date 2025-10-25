using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static Player Instance;

    #region publicVariables

    public GameObject[] coins;
    [Header("Animators")]
    public Animator buttonsAnim;
    public Animator anim;
    public Animator scoreAnim;
    public Animator platformAnim;
    public Animator heartsAnim;
    public int Score {get {return score;} private set{}}
    public int Health {get {return health;} private set{}}
    public bool animIsRunning { get; private set; }

    #endregion
    
    #region privateVariables
    
    private int coinIndex;
    private int score;
    private int health = 3;
    private int tempIndex;
    private bool gameIsRunning;
    private bool canPress;
    private bool addedScore = true;
    private bool canStart = true;
    private bool decreasedHealth;
    private SaveManager saves;
    private Tutorial tutorial;

    #endregion

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() => AllEvents.OnPlayButtonPressed += StartGame;

    private void OnDisable() => AllEvents.OnPlayButtonPressed -= StartGame;

    private void Start()
    {
        saves = SaveManager.Instance;
        Application.targetFrameRate = 60; //30FPS lock on Android problem solution
        tutorial = Tutorial.Instance;
        if (!saves.completedTutorial)
            tutorial.StartCoroutine(tutorial.RunTutorial());
    }

    public void MButton()
    {
        //Main Button - The button that responds to touches on the screen
        if (gameIsRunning && !animIsRunning && canPress && addedScore && coins[coinIndex].activeSelf)
        {
            if (tutorial != null && tutorial.condition != Tutorial.Condition.Tap) return;
            canPress = false;
            decreasedHealth = false;
            addedScore = false;
            anim.SetTrigger("Coin" + (coinIndex + 1));
        }
    }
    
    private void StartGame()
    {
        if (canStart)
        {
            score = 0;
            health = 3;
            AllEvents.RaiseStartGame();
            decreasedHealth = false;
            gameIsRunning = true;
            canPress = true;
            Randomize();
            buttonsAnim.SetBool("GameIsRunning", true);
            scoreAnim.SetBool("GameIsRunning", true);
            platformAnim.SetBool("GameIsRunning", true);
            heartsAnim.SetBool("GameIsRunning", true);
            canStart = false;
        }
    }

    private void EndGame(bool SaveData)
    {
        if (score > saves.bestScore && SaveData)
            saves.bestScore = score;
        AllEvents.RaiseEndGame();
        canStart = true;
        for(int i = 0; i < coins.Length; i++) 
            coins[i].SetActive(false);
        buttonsAnim.SetBool("GameIsRunning", false);
        scoreAnim.SetBool("GameIsRunning", false);
        platformAnim.SetBool("GameIsRunning", false);
        heartsAnim.SetBool("GameIsRunning", false);
        gameIsRunning = false;
        addedScore = true;
    }

    private void Randomize()
    {
        do tempIndex = Random.Range(0, 4);
        while (tempIndex == coinIndex);
        coinIndex = tempIndex;
        for (int i = 0; i < coins.Length; i++)
            coins[i].SetActive(i == coinIndex);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Collecting Coin
        if (other.CompareTag("Coin"))
        {
            if (!addedScore)
            {
                score++;
                addedScore = true;
                AllEvents.RaiseOnCoinCollected();
                other.gameObject.SetActive(false);
                Randomize();
            }
        }
        //Losing Game
        else if (other.CompareTag("Obs"))
        {
            if (!decreasedHealth)
            {
                decreasedHealth = true;
                health--;
                AllEvents.RaiseOnObstacleHit();
                if (health <= 0) EndGame(true);
            }
        }
    }

    #region AnimTriggers
    
    public void RunAnim() =>
        animIsRunning = true;

    public void StopAnim()
    {
        animIsRunning = false;
        canPress = true;
    }
    
    #endregion
}
