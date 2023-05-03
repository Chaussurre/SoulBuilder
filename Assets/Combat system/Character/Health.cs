using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int MaxHP;
    public int Hp { get; private set; }

    public UnityEvent<HpChange> OnHpChange = new();
    public UnityEvent OnDeath = new();

    public struct HpChange
    {
        public int Previous;
        public int Actual;
        public int Max;
    }

    private void Start()
    {
        Hp = MaxHP;
        OnHpChange.Invoke(new()
        {
            Previous = 0,
            Actual = Hp,
            Max = MaxHP,
        });
    }

    public void ChangeHP(int delta)
    {
        var prev = Hp;

        Hp = Mathf.Clamp(Hp + delta, 0, MaxHP);

        OnHpChange.Invoke(new HpChange()
        {
            Previous = prev,
            Actual = Hp,
            Max = MaxHP,
        });

        if (Hp == 0)
            OnDeath.Invoke();
    }


}
