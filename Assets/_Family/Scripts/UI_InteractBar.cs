using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InteractBar : MonoBehaviour
{
    [SerializeField] private Image backFillImage;
    [SerializeField] private TMP_Text percentageText;

    public void UpdateInteractProgress(float zeroToOnePercentage)
    {
        percentageText.text = (int)(zeroToOnePercentage * 100) + " %";
        backFillImage.fillAmount = zeroToOnePercentage;
    }
}
