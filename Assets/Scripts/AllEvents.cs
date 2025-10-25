using System;

public static class AllEvents
{
    public static event Action OnMusicButtonPressed;
    public static event Action OnSFXButtonPressed;
    public static event Action OnPlayButtonPressed;
    public static event Action OnCoinCollected;
    public static event Action OnObstacleHit;
    public static event Action EndGame;
    public static event Action StartGame;

    public static void RaiseOnMusicButtonPressed() =>
        OnMusicButtonPressed?.Invoke();

    public static void RaiseOnSFXButtonPressed() =>
        OnSFXButtonPressed?.Invoke();

    public static void RaiseOnPlayButtonPressed() =>
        OnPlayButtonPressed?.Invoke();

    public static void RaiseOnCoinCollected() =>
        OnCoinCollected?.Invoke();

    public static void RaiseOnObstacleHit() =>
        OnObstacleHit?.Invoke();

    public static void RaiseEndGame() =>
        EndGame?.Invoke();

    public static void RaiseStartGame() =>
        StartGame?.Invoke();
}