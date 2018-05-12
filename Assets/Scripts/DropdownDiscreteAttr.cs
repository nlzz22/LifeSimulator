using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownDiscreteAttr : MonoBehaviour {
    public GameObject discreteAttrField;

    private Dropdown dropdownSelf;
    private Transform parentOfDiscreteAttrField;

	// Use this for initialization
	void Start () {
        dropdownSelf = GetComponent<Dropdown>();
        parentOfDiscreteAttrField = dropdownSelf.transform.Find("Scroll View").Find("Viewport").Find("Content");
	}

    public void UpdateDiscreteAttrValue()
    {
        if (dropdownSelf == null)
        {
            Start();
        }

        int selectedOption = dropdownSelf.value;
        int numFieldsNeeded = selectedOption + 2; // default is 2
        int currentNumChildren = parentOfDiscreteAttrField.childCount;

        // if need to add fields.
        if (numFieldsNeeded > currentNumChildren)
        {
            int numToAdd = numFieldsNeeded - currentNumChildren;
            for (int i = 0; i < numToAdd; i++)
            {
                Instantiate(discreteAttrField, parentOfDiscreteAttrField);
            }
        }
        // if need to deduct/remove fields.
        else if (numFieldsNeeded < currentNumChildren)
        {
            int numToRemove = currentNumChildren - numFieldsNeeded;
            for (int i = 0; i < numToRemove; i++)
            {
                Destroy(parentOfDiscreteAttrField.GetChild(currentNumChildren - 1).gameObject); // remove last child.
                currentNumChildren--;
            }
        }
    }
}
