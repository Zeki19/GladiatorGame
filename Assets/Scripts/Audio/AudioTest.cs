using System;
using UnityEngine;


public class AudioTest : MonoBehaviour
{

    public SO_Sounds playlist;
    private SoundManager sm;

    private void Start()
    {
        sm = ServiceLocator.Instance.GetService<SoundManager>();
    }

    public void Update()
    {

    }
}