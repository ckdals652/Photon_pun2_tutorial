using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerNameInputField : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    private const string PlayerNamePrefKey = "PlayerName";

    void Start()
    {
        string defaultName = string.Empty;
        if (inputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
        }

        if (PlayerPrefs.HasKey(PlayerNamePrefKey))
        {
            defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
            inputField.text = defaultName;
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(PlayerNamePrefKey, value);
    }
}