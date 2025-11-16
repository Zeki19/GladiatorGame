using System.Collections;
using UnityEngine;

public class SpriteEffects
{
    private Coroutine EffectCoroutine;
    private MonoBehaviour _mono;

    public SpriteEffects(MonoBehaviour mono)
    {
        _mono = mono;
    }
    public void Blink(SpriteRenderer sprite,int amount, float frequency, Color activeColor)
    {
        if (EffectCoroutine != null)
            _mono.StopCoroutine(EffectCoroutine);

        EffectCoroutine = _mono.StartCoroutine(DamagedBlink(sprite,amount,frequency,activeColor));
    }

    private IEnumerator DamagedBlink(SpriteRenderer sprite, int amount, float frequency, Color activeColor)
    {
        for (int i = 0; i < amount; i++)
        {
            var oldColor = sprite.color;
            sprite.color = activeColor;
            yield return new WaitForSeconds(frequency);
            sprite.color = oldColor;
            yield return new WaitForSeconds(frequency);
        }
    }
}