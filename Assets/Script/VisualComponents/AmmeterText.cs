using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmeterText : MonoBehaviour
{
    [SerializeField] TextMeshPro AmmeterValue;

    public void UpdateAmmeterValue(double ammeterValue)
    {
        System.Random random = new System.Random();
        float minValue = -0.05f;
        float maxValue = 0.05f;

        float randomFloat = (float)(random.NextDouble() * (maxValue - minValue) + minValue);

        if (ammeterValue >= 100 || ammeterValue <= -100)
        {
            AmmeterValue.text = string.Format("{0:0.##}", (ammeterValue / 1000) + randomFloat * (ammeterValue / 1000)) + " A";
        }
        else if (ammeterValue > 0.001 || ammeterValue < -0.001)
        {
            AmmeterValue.text = string.Format("{0:0.##}", (ammeterValue) + randomFloat * (ammeterValue)) + " mA";
        }
        else
        {
            AmmeterValue.text = string.Format("{0:0.##}", (ammeterValue * 1e3) + randomFloat * (ammeterValue * 1e3)) + " \u03bcA";
        }
    }

    public void InitAmmeterValue()
    {
        AmmeterValue = GetComponent<TextMeshPro>();
    }
}
