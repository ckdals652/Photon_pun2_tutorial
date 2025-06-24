using Photon.Pun;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public string playerPrefabName = "PlayerPrefab";
    public Vector3 spawnPos = new Vector3(0, 1, 0);

    void Start()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsConnectedAndReady)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-5f, 5f);
        float z = Random.Range(-5f, 5f);
        return new Vector3(x, 1, z);
    }
}