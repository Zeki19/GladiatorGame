using System;
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
public interface ICameraCommand
{
    IEnumerator Execute();
}
