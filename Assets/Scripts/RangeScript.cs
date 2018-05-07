using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeScript : MonoBehaviour {

    private InputField currentField;
    private InputField partnerField;

    private bool isStartField; // is either a start field or an end field.
    private int previousValue = -1;

    private static string START_FIELD = "RangeStartField";
    private static string END_FIELD = "RangeEndField";

    // Use this for initialization
    void Start () {
        currentField = GetComponent<InputField>();
        string name = gameObject.name;
        if (name == START_FIELD)
        {
            isStartField = true;
            GameObject partnerObject = transform.parent.Find(END_FIELD).gameObject;
            partnerField = partnerObject.GetComponent<InputField>();
        }
        else if (name == END_FIELD)
        {
            isStartField = false;
            GameObject partnerObject = transform.parent.Find(START_FIELD).gameObject;
            partnerField = partnerObject.GetComponent<InputField>();
        }
        else
        {
            Debug.Log("Error: unknown field name in RangeScript Start() function");
        }
		
	}

    private bool isWithinAcceptableRange(int num)
    {
        return (num >= 0 && num <= 100);
    }

    public void ValidateField()
    {
        bool hasPassedValidation = true;
        int currValue = -1;
        try
        {
            string thisField = currentField.text;
            currValue = Int32.Parse(thisField);

            if (!isWithinAcceptableRange(currValue))
            {
                hasPassedValidation = false;
            }
            
            string otherField = partnerField.text;

            // if value has been set for the other field, check the range validity.
            if (otherField != "")
            {
                int otherValue = Int32.Parse(otherField);
                // if current value is the start field.
                if (isStartField)
                {
                    // if start field > end field, fail the validation.
                    if (currValue > otherValue)
                    {
                        hasPassedValidation = false;
                    }
                }
                else
                // if current value is the end field.
                {
                    // if start field > end field, fail the validation.
                    if (currValue < otherValue)
                    {
                        hasPassedValidation = false;
                    }
                }
            }
        } catch(Exception ) {
            hasPassedValidation = false;
        }

        if (hasPassedValidation)
        {
            previousValue = currValue;
        }
        else
        // fail validation
        {
            // if previous value is not yet set
            if (previousValue == -1)
            {
                currentField.text = "";
            }
            else
            // there is a previous value set
            {
                currentField.text = previousValue.ToString();
            }
        }
    }	
}
