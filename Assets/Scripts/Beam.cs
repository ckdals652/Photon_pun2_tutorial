using Photon.Pun;
using UnityEngine;

public class Beam : MonoBehaviourPun
{
    private PlayerHandler playerHandler;
    private PhotonView targetView;

    private string playerHealthChange = "PlayerHealthChange";

    private void Start()
    {
        playerHandler = transform.parent.gameObject.GetComponent<PlayerHandler>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            targetView = other.GetComponent<PhotonView>();
            
            if (targetView != null && targetView.Owner != null)
            {
                targetView.RPC(playerHealthChange, targetView.Owner,-5f);
            }
        }
    }
}