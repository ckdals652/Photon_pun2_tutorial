using Photon.Pun;
using UnityEngine;

public class Beam : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<PlayerHandler>().playerHealth(-5);
        }
    }
}