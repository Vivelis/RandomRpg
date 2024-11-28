using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string PreviousScene { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}