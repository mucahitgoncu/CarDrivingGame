using UnityEngine;

public class MaintainDistance : MonoBehaviour
{
    public float minDistance = 1.5f; // Minimum distance to maintain
    public Transform car; // Reference to the Car object

    void Update()
    {
        if (car != null)
        {
            float distance = Vector3.Distance(transform.position, car.position);

            if (distance < minDistance)
            {
                Vector3 direction = (transform.position - car.position).normalized;
                transform.position = car.position + direction * minDistance;
            }
        }
    }
}
