using System;
using UnityEngine;

[Serializable]
public class Sound
{
    [SerializeField] public string name;
    [SerializeField] public AudioClip clip;
    [SerializeField] public bool loop;
    [Range(0f, 1f)] public float volume = 1f;
}
