using System.Collections;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Rotate[] obstacles;
    [Header("Settings")]
    [Range(1, 20)]
    public int directionScore = 5;
    [Range(1, 20)]
    public int speedScore = 5;
    public int speedToAdd = 10;
    public int defaultSpeed = 100;
    private int speedStreak;
    private bool speedWasIncreased = true;

    private void OnEnable()
    {
        AllEvents.StartGame += StartGame;
        AllEvents.OnCoinCollected += CheckForChanges;
        AllEvents.EndGame += EndGame;
    }
    
    private void OnDisable()
    {
        AllEvents.StartGame -= StartGame;
        AllEvents.OnCoinCollected -= CheckForChanges;
        AllEvents.EndGame -= EndGame;
    }

    private void CheckForChanges() => StartCoroutine(CheckForChangesRoutine());

    private IEnumerator CheckForChangesRoutine()
    {
        while (Player.Instance.animIsRunning) yield return null;

        //Obstacle direction changer
        int score = Player.Instance.Score;
        if (score % directionScore == 0 && score > 0)
        {
            Rotate firstObs = obstacles[Random.Range(0, obstacles.Length)];
            Rotate secondObs = obstacles[Random.Range(0, obstacles.Length)];
            while (firstObs == secondObs)
                secondObs = obstacles[Random.Range(0, obstacles.Length)];
            firstObs.direction = -firstObs.direction;
            secondObs.direction = -secondObs.direction;
        }

        //Obstacle speed changer
        if (score % speedScore == 0 && score > 0)
        {
            bool increaseSpeed = Random.Range(1, 3) == 1; //if false we'll decrease it
            if (score >= 20)
            {
                if (score == 20) increaseSpeed = false;
                else if (speedWasIncreased == increaseSpeed)
                {
                    speedStreak++;
                    if (speedStreak == 4)
                    {
                        speedStreak = 0;
                        increaseSpeed = !increaseSpeed;
                    }
                }
                else if (speedWasIncreased != increaseSpeed)
                    speedStreak = 0;
                speedWasIncreased = increaseSpeed;
            }
            for (int i = 0; i < obstacles.Length; i++)
            {
                if (score < 20)
                    obstacles[i].speed += speedToAdd;
                else
                    obstacles[i].speed += increaseSpeed ? speedToAdd : -speedToAdd;
            }
        }
    }

    private void StartGame()
    {
        speedWasIncreased = false;
        speedStreak = 0;
    }

    private void EndGame()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].direction.z = i < 2 ? 1 : -1;
            obstacles[i].ResetPos();
            obstacles[i].speed = defaultSpeed;
        }
        speedWasIncreased = false;
        speedStreak = 0;
    }
}