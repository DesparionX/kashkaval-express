using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PriceManager : MonoBehaviour
{
    [Header("Settings")]
    public float priceK;
    public float priceS;
    public float priceM;

    [Header("Price layouts")]
    public InputField priceKt;
    public InputField priceSt;
    public InputField priceMt;

    private string lastText;

    // Load prices
    public void InitializePrices()
    {
        priceK = PlayerPrefs.GetFloat("CheeseKPrice");
        priceS = PlayerPrefs.GetFloat("CheeseSPrice");
        priceM = PlayerPrefs.GetFloat("MilkPrice");

        priceKt.text = (priceK == 0f) ? "Няма цена" : $"{priceK:0.00} лв./бр";
        priceSt.text = (priceS == 0f) ? "Няма цена" : $"{priceS:0.00} лв./бр";
        priceMt.text = (priceM == 0f) ? "Няма цена" : $"{priceM:0.00} лв./бр";
    }

    // If the input field is active save the input text and clear it
    public void ClearInput(InputField input)
    {
        lastText = input.text;
        input.text = "";
    }

    // Update the choosen price
    // in case the field is left empty, last saved price will be displayed
    public void EditPrice(InputField input)
    {
        if (!string.IsNullOrEmpty(input.text))
        {
            SavePrice(input);
            InitializePrices();
        }
        else
        {
            input.text = lastText;
        }
    }

    // Save given price
    private void SavePrice(InputField input)
    {
        if (float.TryParse(input.text, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out float price))
        {
            switch (input.tag)
            {
                case "CheeseK":
                    PlayerPrefs.SetFloat("CheeseKPrice", price);
                    priceKt.text = $"{price:0.00} лв./бр";
                    break;
                case "CheeseS":
                    PlayerPrefs.SetFloat("CheeseSPrice", price);
                    priceSt.text = $"{price:0.00} лв./бр";
                    break;
                case "Milk":
                    PlayerPrefs.SetFloat("MilkPrice", price);
                    priceMt.text = $"{price:0.00} лв./бр";
                    break;
                default:
                    Debug.Log("Something went wrong");
                    break;
            }
        }
        else
        {
            Debug.Log("Thats not a float.");
            input.text = lastText;
        }
    }
}
