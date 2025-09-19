using System;
using System.Collections;
using Player;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene("MainMenu");
    }
}
