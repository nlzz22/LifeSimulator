using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DropdownAttribute : MonoBehaviour {
    public GameObject discreteAttrChancePrefab;

    protected Dropdown dropdownself;
    protected WorldEditControllerScript gameScript;
    protected AttributeScript[] attrDetails;
    protected AttributeScript chosenAttributeDetail;

    private WorldEditControllerScript FindGameScript()
    {
        if (gameScript == null)
        {
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            gameScript = gameController.GetComponent<WorldEditControllerScript>();
            return gameScript;
        }
        else
        {
            return gameScript;
        }
    }

    protected virtual void PopulateDropdown()
    {
        attrDetails = FindGameScript().GetCachedAttributes();
        List<string> tempNames = new List<string>();
        for (int i = 0; i < attrDetails.Length; i++)
        {
            AttributeScript currentAttrDetail = attrDetails[i];
            if (currentAttrDetail == null)
            {
                continue;
            }
            string attributeName = currentAttrDetail.GetAttributeName();
            if (attributeName.Trim() != "")
            {
                tempNames.Add(attributeName);
            }
        
        }

        dropdownself.GetComponent<Dropdown>().AddOptions(tempNames);
    }

    // Use this for initialization
    void Start () {
        dropdownself = GetComponent<Dropdown>();
        PopulateDropdown();
    }

    public virtual void OnDropDownValueChanged()
    {
        chosenAttributeDetail = attrDetails[dropdownself.value];
    }

    protected void RemoveAllChild(GameObject obj)
    {
        int numChild = obj.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            GameObject child = obj.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
