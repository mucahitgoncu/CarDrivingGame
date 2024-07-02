using UnityEngine;

public class OperationManager : MonoBehaviour
{
    public SingletonScoreManager.ScoreOperation operation;
    public int scoreAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            switch (operation)
            {
                case SingletonScoreManager.ScoreOperation.Increase:
                    SingletonScoreManager.Instance.IncreaseScore(scoreAmount);
                    break;
                case SingletonScoreManager.ScoreOperation.Decrease:
                    SingletonScoreManager.Instance.DecreaseScore(scoreAmount);
                    break;
                case SingletonScoreManager.ScoreOperation.Multiply:
                    SingletonScoreManager.Instance.MultiplyScore(scoreAmount);
                    break;
                case SingletonScoreManager.ScoreOperation.Divide:
                    SingletonScoreManager.Instance.DivideScore(scoreAmount);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
