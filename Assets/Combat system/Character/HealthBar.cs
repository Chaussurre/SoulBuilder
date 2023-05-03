using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public RectTransform HealthBarUIParent;
    public RectTransform HealthBarUI;

    public TMP_Text Text;

    public void ChangeHp(Health.HpChange hpChange)
    {
        var y = HealthBarUI.sizeDelta.y;
        var x = hpChange.Max > 0 ? hpChange.Actual * HealthBarUI.rect.width / hpChange.Max : 0;

        HealthBarUI.sizeDelta = new(x, y);

        Text.text = $"{hpChange.Actual}/{hpChange.Max}";
    }
}
