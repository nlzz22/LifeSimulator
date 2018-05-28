using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownAttributeAction : DropdownAttribute {
    public int type = -1;
    public int attributeIndexSelected = -1;
    public int thirdValue = -1;

    private bool toRun = true;

    protected override void PopulateDropdown()
    {
        base.PopulateDropdown();
        // for populating from saved values.
        if (attributeIndexSelected != -1)
        {
            transform.GetComponent<Dropdown>().value = attributeIndexSelected;
            if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
            {
                transform.parent.Find("ContinuousCase").Find("ChangeField").GetComponent<InputField>().text =
                    thirdValue.ToString();
            } else if (type == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
            {
                transform.parent.Find("DiscreteCase").Find("Dropdown").GetComponent<Dropdown>().value =
                    thirdValue;
            }
            HandleSelectedAttribute(attrDetails[attributeIndexSelected], false);
            attributeIndexSelected = -1;
        }
        // for init when adding new condition
        else if (attrDetails.Length > 0)
        {
            HandleSelectedAttribute(attrDetails[0], false);
        }
    }

    public override void OnDropDownValueChanged()
    {
        base.OnDropDownValueChanged();
        HandleSelectedAttribute(chosenAttributeDetail);
    }

    private void HandleSelectedAttribute(AttributeScript attributeDetail, bool toRefresh=true)
    {
        type = attributeDetail.GetAttributeType();
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
            if (toRefresh)
            {
                dropdownOptions.value = 0;
            }
        } else if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
        {
            discreteObj.gameObject.SetActive(false);
            continuousObj.gameObject.SetActive(true);
            if (toRefresh)
            {
                continuousObj.Find("ChangeField").GetComponent<InputField>().text = "";
            }
        }
    }
}
