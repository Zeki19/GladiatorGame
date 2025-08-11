using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[SelectionBase]
public class ResourceBarTracker : MonoBehaviour
{
    //Instant movement of the bar or with smear?
    [Header("Core")]
    [SerializeField] private Image bar;
    [SerializeField] private Image trail;
    [SerializeField] private int resourceMax = 100;
    [SerializeField]private bool startsFull = true;
    
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
    
    [Header("Arc")]
    [SerializeField, Range(0, 360)] private int endDegreeValue = 360;
    
    [Header("Animation")]
    [SerializeField, Range(0f, 5f)] private float trailSpeed = 0.25f;
    private Coroutine _fillRoutine;
    
    [Header("Text")]
    [SerializeField] private TMP_Text textField;
    [SerializeField] private DisplayType valueDisplay = DisplayType.Percentage;
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private Color textColor;
    private enum DisplayType
    {
        [InspectorName("Long (50 | 100)")]
        LongValue,
        [InspectorName("Short (50)")]
        ShortValue,
        [InspectorName("Percent (50%)")]
        Percentage,
        None
    }
    
    
    private int _resourceCurrent = 100;

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
    public void ChangeResourceByAmount(int amount)
    {
        if (_resourceCurrent + amount <= 0) _resourceCurrent = 0;
        
        _resourceCurrent += amount;
        
        TriggerFillAnimation();
    }

    public void SetUp()
    {
        //Max, startsFull,
    }
}
