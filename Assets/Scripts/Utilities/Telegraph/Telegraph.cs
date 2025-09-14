using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class Telegraph : MonoBehaviour
{
    public void StartTelegraph(SpriteRenderer spriteRenderer, float telegraphDuration)
    {
        StartCoroutine(ShowTelegraph(spriteRenderer, telegraphDuration));
    }

    IEnumerator ShowTelegraph(SpriteRenderer spriteRenderer, float telegraphDuration)
    {
        // Fade in or scale up
        float timer = 0f;
        Color startColor = new Color(1, 0, 0, 0);
        Color endColor = new Color(1, 0, 0, 0.5f);

        while (timer < telegraphDuration)
        {
            timer += Time.deltaTime;
            float t = timer / telegraphDuration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}
public enum Shapes
{
    circle,
    square,
    triangle,
    semiCircle
}
