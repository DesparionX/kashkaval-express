using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PriceButtons : MonoBehaviour
{
    public InputField cheeseK;
    public InputField cheeseS;
    public InputField milk;
    public void FocusInput(GameObject button)
    {
        switch (button.name)
        {
            case "btCheeseK":
                cheeseK.ActivateInputField();
                break;
            case "btCheeseS":
                cheeseS.ActivateInputField();
                break;
            case "btMilk":
                milk.ActivateInputField();
                break;
            default:
                break;
        }
    }
    public void FormatInput(InputField input)
    {
        if (string.IsNullOrEmpty(input.text))
            input.text = "0";
        if(int.TryParse(input.text, out int quantity))
        {
            switch (input.tag)
            {
                case "InputCheeseK":
                    cheeseK.text = $"{quantity}";
                    break;
                case "InputCheeseS":
                    cheeseS.text = $"{quantity}";
                    break;
                case "InputMilk":
                    milk.text = $"{quantity}";
                    break;
            }
        }
    }
}
