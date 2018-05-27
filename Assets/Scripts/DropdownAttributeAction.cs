using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownAttributeAction : DropdownAttribute {
    protected override void PopulateDropdown()
    {
        base.PopulateDropdown();
        if (attrDetails.Length > 0)
        {
            HandleSelectedAttribute(attrDetails[0]);
        }
    }

    public override void OnDropDownValueChanged()
    {
        base.OnDropDownValueChanged();
        HandleSelectedAttribute(chosenAttributeDetail);
    }

    private void HandleSelectedAttribute(AttributeScript attributeDetail)
    {
        int type = attributeDetail.GetAttributeType();
        Transform discreteObj = transform.parent.Find("DiscreteCase");
        Transform continuousObj = transform.parent.Find("ContinuousCase");

        if (type == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
        {
            discreteObj.gameObject.SetActive(true);
            continuousObj.gameObject.SetActive(false);
            Dropdown dropdownOptions = discreteObj.Find("Dropdown").GetComponent<Dropdown>();
            dropdownOptions.ClearOptions();
            List<string> newOptions = new List<string>();

            string[] choices = attributeDetail.GetAttributeChoiceNames();
            for (int i = 0; i < choices.Length; i++)
            {
                string choiceName = choices[i];
                newOptions.Add(choiceName);
            }

            dropdownOptions.AddOptions(new List<string>(choices));
            dropdownOptions.value = 0;
        } else if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
        {
            discreteObj.gameObject.SetActive(false);
            continuousObj.gameObject.SetActive(true);
            continuousObj.Find("ChangeField").GetComponent<InputField>().text = "";
        }
    }
}
