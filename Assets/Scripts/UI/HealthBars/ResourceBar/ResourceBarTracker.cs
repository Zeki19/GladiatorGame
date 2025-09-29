using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[SelectionBase]
public class ResourceBarTracker : MonoBehaviour
{
    [Header("Wire")]
    [SerializeField] private Image bar;
    [SerializeField] private Image trail;
    [SerializeField] private TMP_Text textField;
    
    [Header("Settings")]
    [SerializeField] private float resourceMax = 100;
    [SerializeField] private bool startsFull = true;
    private float _resourceCurrent = 100;
    
    [Header("Shape")]
    [SerializeField] private ShapeType shapeOfBar = ShapeType.RectangleHorizontal;
    [SerializeField] private Color barColor;
    
    private enum ShapeType
    {
        [InspectorName("Rectangle (Horizontal)")]
        RectangleHorizontal,
        [InspectorName("Rectangle (Vertical)")]
        RectangleVertical,
        [InspectorName("Circle")]
        Circle,
        Arc
    }
    
    [Header("If circular shape")]
    [SerializeField, Range(0, 360)] private int endDegreeValue = 180;
    
    [Header("Animation")]
    [SerializeField, Range(0f, 1f)] private float trailSpeed = 0.5f;
    private Coroutine _fillRoutine;
    
    [Header("Text")]
    [SerializeField] private DisplayType valueDisplay = DisplayType.Percentage;
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private Color textColor;
    
    public enum DisplayType
    {
        [InspectorName("Long (50 | 100)")]
        LongValue,
        [InspectorName("Short (50)")]
        ShortValue,
        [InspectorName("Percent (50%)")]
        Percentage,
        None
    }
    
    private void OnValidate()
    {
        SetUpBar();
    }
    private void Start()
    {
        TriggerFillAnimation();
    }
    private void SetUpBar()
    {
        switch (shapeOfBar)
        {
            case ShapeType.RectangleHorizontal:
                bar.fillMethod = Image.FillMethod.Horizontal;
                trail.fillMethod = Image.FillMethod.Horizontal;
                break;
            case ShapeType.RectangleVertical:
                bar.fillMethod = Image.FillMethod.Vertical;
                trail.fillMethod = Image.FillMethod.Vertical;
                break;
            case ShapeType.Circle:
            case ShapeType.Arc:
                bar.fillMethod = Image.FillMethod.Radial360;
                trail.fillMethod = Image.FillMethod.Radial360;
                bar.fillOrigin = (int)Image.Origin360.Top;
                trail.fillOrigin = (int)Image.Origin360.Top;
                break;
        }
        
        bar.color = barColor;
        
        textField.font = font;
        textField.color = textColor;

        trail.enabled = trailSpeed != 0;
        
        UpdateResourceBar();
    }
    private void UpdateResourceBar()
    {
        _resourceCurrent = startsFull ? resourceMax : 0;
        
        float fillAmount;
        if (shapeOfBar == ShapeType.Arc)
        {
            fillAmount = CalculateCircularFillAmount();
        }
        else
        {
            fillAmount = (float) _resourceCurrent / resourceMax;
        }
        
        bar.fillAmount = fillAmount;
        trail.fillAmount = fillAmount;
        
        SetTextField();
    }
    
    private void TriggerFillAnimation()
    {
        float targetFill = CalculateTargetFill();

        if (Mathf.Approximately(bar.fillAmount, targetFill)) return;
        if (_fillRoutine != null) StopCoroutine(_fillRoutine);

        _fillRoutine = StartCoroutine(SmoothlyTransitionToNewValue(targetFill));
        SetTextField();
    }
    
    private IEnumerator SmoothlyTransitionToNewValue(float targetFill)
    {
        bar.fillAmount = targetFill;
        
        while (trail.fillAmount > targetFill)
        {
            trail.fillAmount = Mathf.MoveTowards(trail.fillAmount, targetFill, trailSpeed * Time.deltaTime);
            yield return null;
        }
        trail.fillAmount = targetFill;
        
        
    }
    private float CalculateTargetFill()
    {
        if (shapeOfBar == ShapeType.Arc) return CalculateCircularFillAmount();

        return (float)_resourceCurrent / resourceMax;
    }
    private float CalculateCircularFillAmount()
    {
        float fraction = (float) _resourceCurrent / resourceMax;
        float fillRange = endDegreeValue / 360f;
        
        return fillRange * fraction;
    }
    private void SetTextField()
    {
        switch (valueDisplay)
        {
            case DisplayType.LongValue:
                textField.SetText($"{_resourceCurrent}/{resourceMax}");
                break;
            case DisplayType.ShortValue:
                textField.SetText($"{_resourceCurrent}");
                break;
            case DisplayType.Percentage:
                float percentage = ((float) _resourceCurrent / resourceMax) * 100;
                textField.SetText($"{Mathf.RoundToInt(percentage)} %");
                break;
            case DisplayType.None:
                textField.SetText(string.Empty);
                break;
        }
    }
    
    //Public methods
    public void ChangeResourceByAmount(float amount)
    {
        _resourceCurrent = Mathf.Clamp(_resourceCurrent + amount, 0f, resourceMax);
        TriggerFillAnimation();
    }

    public void ChangeResourceToAmount(float amount)
    {
        _resourceCurrent = Mathf.Clamp(amount, 0f, resourceMax);
        TriggerFillAnimation();
    }
    public void SetUp(float max, bool startsAt100Percent)
    {
        resourceMax = max;
        startsFull = startsAt100Percent;
        
        UpdateResourceBar();
    }

    public void SetUp(float max, bool startsAt100Percent, Color color)
    {
        resourceMax = max;
        startsFull = startsAt100Percent;
        bar.color = color;
        
        UpdateResourceBar();
    }

    public void SetTextField(DisplayType displayType)
    {
        valueDisplay = displayType;
        SetTextField();
    }
}
