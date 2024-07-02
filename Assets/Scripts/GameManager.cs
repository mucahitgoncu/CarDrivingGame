using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int gold;
    private const string GoldKey = "Gold";

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

        gold = PlayerPrefs.GetInt(GoldKey, 0);
    }

    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            PlayerPrefs.SetInt(GoldKey, gold);
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }
}
