using Entities;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class EntityView : MonoBehaviour, ILook
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected EntityManager manager;
    
    public abstract void LookDir(Vector2 dir);
    public abstract void PlayStateAnimation(StateEnum state);
}
