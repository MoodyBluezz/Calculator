using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using B83.ExpressionParser;

public class CalculaterController : MonoBehaviour
{
    //public TMP_InputField display;
    public TextMeshProUGUI display;
    //public TMP_InputField secondaryDisplay;
    [SerializeField] List<float> digits = new List<float>(11) { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
    [SerializeField] List<float> operators = new List<float>(6) { '+', '-', '×', '÷' };
    bool enteredOperator = false;
    bool enteredDot = false;
    bool resetUponDigitInput = false;
    bool isNegative = false;

/*    private void Start()
    {
        var parser = new ExpressionParser();
        Expression exp = parser.EvaluateExpression("(2+1)*2+2/2");
        Debug.Log("Result: " + exp.Value);  // prints: "Result: 522"
    }*/

    public void DigitPushed(string digit)
    {
        if (resetUponDigitInput)
        {
            resetUponDigitInput = false;
            display.text = "0";
        }
        if (enteredDot == true && digit == ".")
            return;
        if (digit == ".")
            enteredDot = true;
        if (display.text == "0" && digit != ".")
            display.text = display.text.Substring(0, display.text.Length - 1);
        display.text += digit;
        enteredOperator = false;
    }

    public void OperatorPushed(string operation)
    {
        if (enteredOperator == true)
            return;
        display.text += operation;
        enteredOperator = true;
        enteredDot = false;
        resetUponDigitInput = false;
    }

    public void RemoveDigit()
    {
        resetUponDigitInput = false;
        if (display.text.Length - 1 <= 0)
        {
            display.text = "0";
            return;
        }
        if (display.text[display.text.Length - 1] == '.')
            enteredDot = false;
        display.text = display.text.Substring(0, display.text.Length - 1);
    }

    public void CalculateAnswer()
    {
        string entry = string.Empty;
        int expressionLength = display.text.Length;
        List<float> expressionEntries = new List<float>();
        List<float> expressionOperators = new List<float>();
        int mdNumber = 0;
        var parser = new ExpressionParser();
        for (int x = 0; x < expressionLength; x++)
        {
            if (digits.Contains(display.text[0]))
            {
                entry += display.text[0].ToString();
                if (display.text.Length == 1)
                    expressionEntries.Add(float.Parse(entry, System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (operators.Contains(display.text[0]))
            {
                if (display.text[0] == '×' || display.text[0] == '÷')
                    mdNumber++;

                if (display.text[0].ToString() == "-")
                    entry = $"{0 + entry}";

                expressionEntries.Add(float.Parse(entry, System.Globalization.CultureInfo.InvariantCulture));
                expressionOperators.Add(display.text[0]);
                entry = string.Empty;
            }
            display.text = display.text.Substring(1);
        }

        for (int y = 0; y < mdNumber; y++)
        {
            for (int w = 0; w < Mathf.Infinity; w++)
            {
                float orderOfOperationResult;
                if (expressionOperators[w] == '×')
                {
                    orderOfOperationResult = expressionEntries[w] * expressionEntries[w + 1];
                    expressionEntries[w] = orderOfOperationResult;
                    expressionEntries.Remove(expressionEntries[w + 1]);
                    expressionOperators.Remove(expressionOperators[w]);
                    break;
                }
                else if (expressionOperators[w] == '÷')
                {
                    orderOfOperationResult = expressionEntries[w] / expressionEntries[w + 1];
                    expressionEntries[w] = orderOfOperationResult;
                    expressionEntries.Remove(expressionEntries[w + 1]);
                    expressionOperators.Remove(expressionOperators[w]);
                    break;
                }
            }
        }

        float result = expressionEntries[0];
        for (int z = 0; z < expressionOperators.Count; z++)
        {
            if (expressionOperators[z] == '+')
                result += expressionEntries[z + 1];
            else if (expressionOperators[z] == '-')
                result -= expressionEntries[z + 1];
        }

        Expression exp = parser.EvaluateExpression(result.ToString());

        display.text = exp.Value.ToString();
        resetUponDigitInput = true;
    }

    public void ClearAllEntries() // C
    {
        display.text = "0";
        enteredOperator = false;
        enteredDot = false;
        resetUponDigitInput = false;
    }

    private void ClearCurrentEntry() // CE
    {
        for (int x = 0; x < Mathf.Infinity; x++)
        {
            if (operators.Contains(display.text[display.text.Length - 1]))
                break;
            else if (display.text.Length - 1 == 0)
            {
                display.text = "0";
                break;
            }
            else if (digits.Contains(display.text[display.text.Length - 1]))
                display.text = display.text.Substring(0, display.text.Length - 1);
        }
        enteredOperator = false;
        enteredDot = false;
        resetUponDigitInput = false;
    }

    private float ReturnEndEntry() // find and return the entry at the end
    {
        string entryString = string.Empty;
        for (int x = 0; x < display.text.Length; x++)
        {
            entryString += display.text[x];
            if (operators.Contains(display.text[x]))
                entryString = string.Empty;
        }
        float entry = float.Parse(entryString, System.Globalization.CultureInfo.InvariantCulture);
        return entry;
    }

    private void ChangeEndEntry(float calculation) // apply the calculation
    {
        if (display.text == "0")
        {
            display.text = calculation.ToString();
            return;
        }
        display.text += calculation.ToString();
    }

    public void EnterPI()
    {
        ClearCurrentEntry();
        if (display.text == "0")
            display.text = string.Empty;
        display.text += "3.141593";
        resetUponDigitInput = true;
    }

    public void CalculateTheSquareRoot()
    {
        float calculation = Mathf.Sqrt(ReturnEndEntry());
        ClearCurrentEntry();
        ChangeEndEntry(calculation);
        resetUponDigitInput = true;
    }

    public void CalculateTheSquareRootYX()
    {
        float calculation = Mathf.Sqrt(ReturnEndEntry());
        ClearCurrentEntry();
        ChangeEndEntry(calculation);
        resetUponDigitInput = true;
    }

    public void CalculateToThePowerOfTwo()
    {
        float calculation = Mathf.Pow(ReturnEndEntry(), 2);
        ClearCurrentEntry();
        ChangeEndEntry(calculation);
        resetUponDigitInput = true;
    }

/*    public void CalculateToThePowerOfX()
    {
        if (!string.IsNullOrEmpty(secondaryDisplay.text))
        {
            float calculation = Mathf.Pow(ReturnEndEntry(), float.Parse(secondaryDisplay.text));
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }*/

    public void CalculateSin()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float getRadian = ReturnEndEntry() * Mathf.PI / 180;
            float calculation = Mathf.Sin(getRadian);
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }
    public void CalculateASin()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float calculation = Mathf.Asin(ReturnEndEntry());
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }

    public void CalculateCos()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float getRadian = ReturnEndEntry() * Mathf.PI / 180;
            float calculation = Mathf.Cos(getRadian);
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }
    public void CalculateACos()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float calculation = Mathf.Acos(ReturnEndEntry());
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }

    public void CalculateTan()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float getRadian = ReturnEndEntry() * Mathf.PI / 180;
            float calculation = Mathf.Tan(getRadian);
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }
    public void CalculateATan()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float calculation = Mathf.Atan(ReturnEndEntry());
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }

    public void CalculateLn()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float calculation = Mathf.Log(ReturnEndEntry());
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }

    public void CalculateLogarithm()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float calculation = Mathf.Log10(ReturnEndEntry());
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }

/*    public void CalculateLogarithmYX()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float calculation = Mathf.Log(float.Parse(secondaryDisplay.text), ReturnEndEntry());
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }*/

    public void CalculateExponent()
    {
        if (!string.IsNullOrEmpty(display.text))
        {
            float calculation = Mathf.Exp(ReturnEndEntry());
            ClearCurrentEntry();
            ChangeEndEntry(calculation);
            resetUponDigitInput = true;
        }
    }

    public void CalculateFactorial()
    {
        int numberInt = int.Parse(ReturnEndEntry().ToString());
        int result = numberInt;

        for (int i = 1; i < numberInt; i++)
        {
            result = result * i;
        }

        float calculation = result;
        ClearCurrentEntry();
        ChangeEndEntry(calculation);
        resetUponDigitInput = true;
    }

    public void CalculateScientificNotation()
    {
        /*int numberToConvert = (int)ReturnEndEntry();
        string numerals = numberToConvert.ToString();
        display.text = numerals.Substring(0, 1) + "." + numerals.Substring(1) + "10^" + numerals.Length.ToString();
        float calculation = float.Parse(display.text, System.Globalization.CultureInfo.InvariantCulture);
        ClearCurrentEntry();
        ChangeEndEntry(calculation);*/

        float calculation = Mathf.Pow(10, ReturnEndEntry());
        ClearCurrentEntry();
        ChangeEndEntry(calculation);
        resetUponDigitInput = true;
    }

    public void CalculateOver1()
    {
        float calculation = (1 / ReturnEndEntry());
        ClearCurrentEntry();
        ChangeEndEntry(calculation);
        resetUponDigitInput = true;
    }

    public void NegativeValue()
    {
        isNegative = true;
        float calculation = -1 * ReturnEndEntry();
        ClearCurrentEntry();
        ChangeEndEntry(calculation);
        resetUponDigitInput = true;
    }

    public void AddOpeningBracket()
    {
        display.text += "(";
    }

    public void AddClosingBracket()
    {
        display.text += ")";
    }
}