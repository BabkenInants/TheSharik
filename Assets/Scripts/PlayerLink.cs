using UnityEngine;

public class PlayerLink : MonoBehaviour
{
    private Player player;

    private void Start() => player = Player.Instance;

    public void RunAnim() => player.RunAnim();

    public void StopAnim() => player.StopAnim();
}
