using UnityEngine;

public class HealerNPC : MonoBehaviour
{
    [Header("Message de soin")]
    [SerializeField] private string healMessage = "Tous les joueurs ont �t� soign�s !";

    public void HealPlayers()
    {
        Debug.Log(healMessage);
    }
}
