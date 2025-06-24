using Photon.Pun;
using UnityEngine;

public class PlayerHandler : MonoBehaviourPun
{
    private PlayerStatus playerStatus = new PlayerStatus();

    public void playerHealth(float health)
    {
        playerStatus.Health += health;
        Debug.Log(playerStatus.Health);
    }
}