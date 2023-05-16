using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Atom : MonoBehaviour
{
    [HideInInspector] public AtomType Type;

    public UnityEvent OnCall = new();

    public void Apply()
    {
        OnCall.Invoke();
    }
}
