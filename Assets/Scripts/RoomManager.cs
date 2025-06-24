using Photon.Pun;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public string playerPrefabName = "PlayerPrefab";
    public Vector3 spawnPos = new Vector3(0, 1, 0);

    public void Start()
    {
        //player생성
        PhotonNetwork.Instantiate(playerPrefabName, spawnPos, Quaternion.identity);
    }
}