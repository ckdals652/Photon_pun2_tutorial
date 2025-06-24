using Photon.Pun;
using UnityEngine;

public class Beam : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            PhotonView targetView = other.GetComponent<PhotonView>();
            PlayerHandler targetPlayerHandler = targetView.GetComponent<PlayerHandler>();
            
            if (targetView != null && targetView.Owner != null)
            {
                photonView.RPC("PlayerDamage", targetView.Owner,targetPlayerHandler);
            }
        }
    }

    [PunRPC]
    public void PlayerDamage(PlayerHandler player)
    {
        // 이 함수는 맞은 플레이어 본인만 실행함
        player.playerHealth(-5);
        Debug.Log("Player damage : "+ photonView.ViewID);
    }
}