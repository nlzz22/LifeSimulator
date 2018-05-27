using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownAttributeCondition : DropdownAttribute {
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
            Transform discreteContent = discreteObj.Find("Scroll View").Find("Viewport").Find("Content");
            RemoveAllChild(discreteContent.gameObject);
            string[] choices = attributeDetail.GetAttributeChoiceNames();
            for (int i = 0; i < choices.Length; i++)
            {
                GameObject choiceField = Instantiate(discreteAttrChancePrefab, discreteContent);
                string choiceName = choices[i];
                choiceField.transform.Find("Text").GetComponent<Text>().text = choiceName;
            }
        } else if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
        {
            discreteObj.gameObject.SetActive(false);
            continuousObj.gameObject.SetActive(true);
            GameObject[] objToReset = new GameObject[4];
            objToReset[0] = continuousObj.Find("X(Value)1").gameObject;
            objToReset[1] = continuousObj.Find("Y(Percent)1").gameObject;
            objToReset[2] = continuousObj.Find("X(Value)2").gameObject;
            objToReset[3] = continuousObj.Find("Y(Percent)2").gameObject;
            for (int i = 0; i < objToReset.Length; i++)
            {
                objToReset[i].GetComponent<InputField>().text = "";
            }
        }
    }
}
