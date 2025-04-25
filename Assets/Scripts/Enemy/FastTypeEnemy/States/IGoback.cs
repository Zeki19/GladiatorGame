using System;
using UnityEngine;

public interface IGoback 
{
    void Goback();
    Action OnGoBack { get; set; }
}
