using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmeterText : MonoBehaviour
{
    [SerializeField] TextMeshPro AmmeterValue;

    public void UpdateAmmeterValue(double ammeterValue)
    {
<<<<<<< HEAD
        /*if (ammeterValue >= 100000 || ammeterValue <= -100000)
        {
            AmmeterValue.text = string.Format("{0:0.##}", ammeterValue / 1000000) + "A";
        }
        else*/ if (ammeterValue >= 100 || ammeterValue <= -100)
        {
            AmmeterValue.text = string.Format("{0:0.##}", ammeterValue / 1000) + "A";
        }
        else
        {
            AmmeterValue.text = string.Format("{0:0.##}", ammeterValue) + " mA";
        }

=======
        if(ammeterValue >= 100 || ammeterValue <= -100) { AmmeterValue.text = string.Format("{0:0.##}", ammeterValue / 1000) + " A"; }
        else if (ammeterValue <= 1 && ammeterValue>=-1) { AmmeterValue.text = string.Format("{0:0.##}", ammeterValue*1e7) + " \u03bcA"; }
        else { AmmeterValue.text = string.Format("{0:0.##}", ammeterValue) + " mA"; }
>>>>>>> DEVELOPERX-coder-main
    }

    public void InitAmmeterValue()
    {
        AmmeterValue = GetComponent<TextMeshPro>();
    }
}
