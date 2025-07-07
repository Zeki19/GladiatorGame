using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Sounds/Playlist")]
public class SO_Sounds : ScriptableObject
{
    [SerializeField] public Sound[] sounds;
}