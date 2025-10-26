using System;
using UnityEngine;

public class PlaySimpleOnEnableAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationName;
    public float animationSpeed;

    private void OnEnable()
    {
        animator.speed = animationSpeed;
        animator.Play(animationName);
    }
}
