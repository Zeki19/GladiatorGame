using UnityEngine;

public class ChompEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void PlayEffect()
    {
        spriteRenderer.enabled = true;
        animator.SetTrigger("Chomp");
    }

    public void StopEffect()
    {
        spriteRenderer.enabled = false;
    }
}
