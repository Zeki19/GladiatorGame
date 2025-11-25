using UnityEngine;

public class GrabHighlight : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    private MaterialPropertyBlock _mpb;

    [SerializeField] private Shader outlineShader;

    private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
    private static readonly int OutlineSize = Shader.PropertyToID("_OutlineSize");

    private Material _runtimeMaterial;
    private bool _isHighlighted;
    void Awake()
    {
        if(!_renderer)
            Destroy(gameObject);

        _mpb = new MaterialPropertyBlock();

        if (!outlineShader) outlineShader = Shader.Find("Custom/SpriteOutline");

        _runtimeMaterial = new Material(outlineShader);
        _runtimeMaterial.mainTexture = _renderer.sprite.texture;

        _renderer.material = _runtimeMaterial;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        if (Vector3.Distance(col.gameObject.transform.position, transform.position) > 2) 
        {
            if (!_isHighlighted)
                return;
            DisableOutline();
            _isHighlighted = false;
        }
        else 
        {
            if (_isHighlighted)
                return;
            EnableOutline(Color.white, 1f);
            _isHighlighted = true;
        }
    }

    public void EnableOutline(Color color, float size)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(OutlineColor, color);
        _mpb.SetFloat(OutlineSize, size);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void DisableOutline()
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(OutlineSize, 0f);
        _renderer.SetPropertyBlock(_mpb);
    }
}
