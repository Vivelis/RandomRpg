using UnityEngine;

public class HealerNPC : MonoBehaviour
{
    [Header("Message de soin")]
    [SerializeField] private string healMessage = "Tous les joueurs ont été soignés !";

    public void HealPlayers()
    {
        Debug.Log(healMessage);
    }
}
