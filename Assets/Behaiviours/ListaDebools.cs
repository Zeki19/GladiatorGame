
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ListOfBools", order = 1)]
public class ListaDebools : ScriptableObject
{
    public List<ListaDeboolsSO> list;
}
[Serializable]
public class ListaDeboolsSO
{
    public bool B;
    public string Name;
}