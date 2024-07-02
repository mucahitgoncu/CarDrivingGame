using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldCollector : MonoBehaviour
{
    [SerializeField] private int finishGoldAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (gameObject.CompareTag("Finish"))
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddGold(finishGoldAmount);
                    SceneManager.LoadScene("SceneMainMenu"); // MainMenu sahnesine geçiþ yap
                }
                else
                {
                    Debug.LogError("GameManager instance is null. Make sure GameManager is added to the scene.");
                }
            }
        }
    }
}
