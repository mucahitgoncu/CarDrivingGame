using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // Takip edilecek nesne
    public float speed = 5f; // Takip hýzý
    public float stopDistance = 1f; // Durdurma mesafesi

    void Start()
    {
        // Hedef nesneyi sahnede tag ile bul
        target = GameObject.FindWithTag("Car").transform;
    }

    void Update()
    {
        if (target != null)
        {
            // Hedefe doðru hareket
            Vector3 direction = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            // Hedefe ulaþýldýðýnda durma mesafesi
            if (direction.magnitude <= stopDistance)
            {
                // Durdurma mesafesine ulaþýldýðýnda hareket etme
                return;
            }
            else
            {
                // Hedefe doðru hareket et
                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            }
        }
    }
}