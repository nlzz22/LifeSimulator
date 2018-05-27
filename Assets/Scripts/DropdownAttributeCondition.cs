using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownAttributeCondition : DropdownAttribute {
    public int type = -1;
    public int attributeIndexSelected = -1;
    public float[] percentagesFeed;

    protected override void PopulateDropdown()
    {
        base.PopulateDropdown();
        // for populating from saved values.
        if (attributeIndexSelected != -1)
        {
            if (type == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
            {
                transform.GetComponent<Dropdown>().value = attributeIndexSelected;

                Transform contentParent = transform.parent.Find("DiscreteCase").Find("Scroll View").
                            Find("Viewport").Find("Content");
                for (int k = 0; k < percentagesFeed.Length; k++)
                {
                    Transform child = contentParent.GetChild(k);
                    child.Find("InputField").GetComponent<InputField>().text = percentagesFeed[k].ToString();
                }
            } else if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
            {
                HandleSelectedAttribute(attrDetails[attributeIndexSelected], false);
            }

            // reset state
            attributeIndexSelected = -1;
            percentagesFeed = null;
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

            if (toRefresh)
            {
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
}
