using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unitText;
    [SerializeField] Slider healthSlider;

    public void SetHUD(Unit unit)
    {
        unitText.text = unit.unitName + " Lvl." + unit.unitLevel;

        healthSlider.maxValue = unit.unitMaxHP;
        healthSlider.value = unit.unitCurrentHP;
    }

    public void SetHP(int hp)
    {
        healthSlider.value = hp;
    }

}
