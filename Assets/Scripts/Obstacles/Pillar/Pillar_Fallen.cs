        using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class Pillar_Fallen : MonoBehaviour, IPillar
{ 
    [Header("Fall Setup")]
    [SerializeField] private int pillarLength = 3;
    [SerializeField] private float fallingTime = 1f;
    [SerializeField] private AnimationCurve fallCurve = AnimationCurve.EaseInOut(0,0, 1,1);

    private PillarContext _context;
    private List<Vector3> _occupiedPositions = new List<Vector3>();
    private Vector3 _chosenDir;
    private ArenaPainter _painter;
    
    private Vector3 _startScale, _targetScale;
    private Vector3 _startPos,   _targetPos;
    private List<Collider2D> _ignoreColliders;
    
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField]private BlinkValues blinkDamage;
    private SpriteEffects _blink;


    public event Action OnDamaged;

    public void SpawnPillar(PillarContext context)
    {
        _context  = context;
        _painter  = ServiceLocator.Instance.GetService<ArenaPainter>();
        
        _chosenDir = GetRandomCardinal();
        
        _occupiedPositions.Clear();
        Vector3 originPos = _context.Origin.position;
        for (int i = 0; i < pillarLength; i++)
        {
            _occupiedPositions.Add(originPos + _chosenDir * (i));
        }
        
        foreach (var p in _occupiedPositions)
        {
            //_painter.PaintArenaNoRotation(p, "Shadow");
        }
        
        ComputeTargets(originPos);
        
        context.OccupiedSpaces.Clear();
        context.OccupiedSpaces.AddRange(_occupiedPositions);
        
        //transform.localScale = _startScale;
        //transform.position   = _startPos;

        //transform.localScale = _targetScale;
        transform.position = _targetPos;
        ServiceLocator.Instance.GetService<NavMeshService>().RebuildNavMesh();
        //StartCoroutine(FallRoutine()); <--- Change this to make it use the pillar falling animation.

        OnDamaged += Blink;
    }

    public void DestroyPillar(PillarContext context)
    {
        OnDamaged = null;
        Destroy(gameObject);
    }

    private IEnumerator FallRoutine()
    {
        float t = 0f;
        while (t < fallingTime)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / fallingTime);
            u = fallCurve.Evaluate(u);

            transform.localScale = Vector3.LerpUnclamped(_startScale, _targetScale, u);
            transform.position   = Vector3.LerpUnclamped(_startPos,   _targetPos,   u);

            yield return null;
        }

        transform.localScale = _targetScale;
        transform.position   = _targetPos;

        foreach (var p in _occupiedPositions)
        {
            _painter.ClearPaint(p);
        }
    }

    private void ComputeTargets(Vector3 originPos)
    {

        _startScale = new Vector3(1f, 1f, 1f);
        _startPos   = originPos;
        
        _targetScale = _startScale;
        if (Mathf.Abs(_chosenDir.x) > 0.01f)
        {
            _targetScale.x = pillarLength;
        }
        else
        {
            //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 90, transform.rotation.w);
            _targetScale.x = pillarLength;
        }

        float halfExtentFromCenter = ((pillarLength - 1) * 0.5f);
        //_targetPos = originPos + Vector3.right * halfExtentFromCenter;
        _targetPos = originPos + Vector3.right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_ignoreColliders.Any(collider => collider == other))
        {
            return;
        }
        OnDamaged?.Invoke();
    }
    public void AddIgnorePillar(List<Collider2D> colliders)
    {
        _ignoreColliders = colliders;
    }

    private void Blink()
    {
        _blink = new SpriteEffects(this);
        _blink.Blink(sprite, blinkDamage.amount, blinkDamage.frequency, blinkDamage.blinkActive);
    }

    private static Vector3 GetRandomCardinal()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: return Vector3.right;
            case 1: return Vector3.left;
            case 2: return Vector3.up;
            default: return Vector3.down;
        }
    }
    private void OnDestroy()
    {
        ServiceLocator.Instance.GetService<NavMeshService>().RebuildNavMesh();
    }
}
