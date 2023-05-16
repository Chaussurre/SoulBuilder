using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "atom", menuName = "Rune System/Create Atom")]
public class AtomType : ScriptableObject
{
    public Atom Prefab;

    public string AtomName;
}
