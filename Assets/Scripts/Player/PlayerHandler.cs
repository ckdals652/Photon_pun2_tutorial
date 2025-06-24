using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private PlayerStatus playerStatus = new PlayerStatus();

    public void playerHealth(float health)
    {
        playerStatus.Health += health;
        Debug.Log(playerStatus.Health);
    }
}
