using UnityEngine;
using System.Collections.Generic;

public class FadeOut : MonoBehaviour
{
    public List<CanvasGroup> canvasGroups = new List<CanvasGroup>(); // Birden fazla CanvasGroup için liste
    public float duration = 6f; // 6 saniye
    private float timer = 0f;

    void Start()
    {
        // Eðer canvasGroups listesi boþsa, bu GameObject'in CanvasGroup bileþenini ekleyin
        if (canvasGroups.Count == 0)
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg != null)
            {
                canvasGroups.Add(cg);
            }
        }
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float alphaValue = Mathf.Lerp(1, 0, timer / duration);
            foreach (CanvasGroup cg in canvasGroups)
            {
                if (cg != null)
                {
                    cg.alpha = alphaValue;
                }
            }
        }
    }
}
