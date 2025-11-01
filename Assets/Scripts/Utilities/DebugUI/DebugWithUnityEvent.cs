using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DebugWithUnityEvent : MonoBehaviour
{
    public MethodCaller methodToCall;
    public string DebugName;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI data;

    private void Start()
    {
        Name.text = DebugName;
    }

    void Update()
    {
        var result = methodToCall.Invoke();
        data.text= (string)(result ?? "null");
    }
}
[Serializable]
public class MethodCaller
{
    [SerializeField] private MonoBehaviour target;
    [SerializeField] private string methodName;

    public MonoBehaviour Target => target;
    public string MethodName => methodName;

    public object Invoke(params object[] parameters)
    {
        if (target == null || string.IsNullOrEmpty(methodName))
        {
            Debug.LogWarning("MethodCaller: No target or method selected.");
            return null;
        }

        MethodInfo method = target.GetType().GetMethod(
            methodName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        if (method == null)
        {
            Debug.LogWarning($"MethodCaller: Method '{methodName}' not found on {target.name}");
            return null;
        }

        return method.Invoke(target, parameters);
    }
}