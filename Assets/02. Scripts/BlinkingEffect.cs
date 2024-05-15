using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingEffect : MonoBehaviour
{
    public GameObject panel;
    public float blinkSpeed = 1f;
    public float minAlpha = 0.1f;
    public float maxAlpha = 1f;

    private bool isBlinking = false;
    private float currentAlpha;
    private float timer = 0f;

    void Update()
    {
        if (isBlinking)
        {
            timer += Time.deltaTime;
            currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(timer * blinkSpeed, 1f));

            panel.GetComponent<CanvasGroup>().alpha = currentAlpha;
        }
    }

    public void StartBlinking()
    {
        isBlinking = true;
        timer = 0f;
    }

    public void StopBlinking()
    {
        isBlinking = false;
        panel.GetComponent<CanvasGroup>().alpha = maxAlpha;
    }
}
