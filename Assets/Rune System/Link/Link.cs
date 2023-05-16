using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Link : MonoBehaviour
{
    public Rune Input;
    public Rune Output;

    [HideInInspector] public float length;
    public float speed;

    public List<Atom> Atoms = new();
    public Dictionary<Atom, float> Distances = new();

    private void Update()
    {
        length = Vector3.Distance(Input.transform.position, Output.transform.position);
        float prev = float.MaxValue;
        float minDist = RuneManager.Singleton.AtomMinDistance;
        for(int i = 0; i < Atoms.Count; i++)
        {
            var atom = Atoms[i];
            var dist = Distances[atom] + Time.deltaTime * speed;
            if (dist >= length) //atom pass finish line
            {
                if (Output.AddAtom(atom, this))
                {
                    Distances.Remove(atom);
                    Atoms.RemoveAt(0);
                    i--; //we don't increase index this loop
                }
                else
                {
                    prev = length;
                    Distances[atom] = length;
                }
            }
            else //update atom to new pos
            {
                if (dist >= prev - minDist)
                    dist = prev - minDist;
                prev = dist;
                Distances[atom] = dist;
            }
        }
    }

    public bool isOpen() //A link is open if a new atom can be pushed to it
    {
        return lastAtomDist() > RuneManager.Singleton.AtomMinDistance;
    }

    public float lastAtomDist()
    {
        if (Atoms.Count <= 0)
            return float.MaxValue;

        return Distances[Atoms.Last()];
    }

    public bool PushAtom(Atom atom)
    {
        if (isOpen())
        {
            atom.gameObject.SetActive(true);
            atom.transform.parent = transform;
            Atoms.Add(atom);
            Distances.Add(atom, 0f);
            return true;
        }

        return false;
    }
}