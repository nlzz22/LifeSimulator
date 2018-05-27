using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownCondition : MonoBehaviour
{
    private Dropdown dropdownself;
    private GameObject conditionalAttachment;

    [SerializeField]
    private GameObject attributeConditionalPrefab;

    // to correspond with options at the dropdown object.
    private static int ATTRIBUTE = 1;

    private void Start()
    {
        dropdownself = GetComponent<Dropdown>();
        conditionalAttachment = dropdownself.gameObject.transform.Find("ConditionalAttachment").gameObject;
    }

    private void RemoveAllChild(GameObject obj)
    {
        int numChild = obj.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            GameObject child = obj.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    public void DropdownValueChanged()
    {
        if (dropdownself == null)
        {
            Start();
        }

        int selectedValue = dropdownself.value;
        if (selectedValue == ATTRIBUTE)
        {
            RemoveAllChild(conditionalAttachment);
            Instantiate(attributeConditionalPrefab, conditionalAttachment.transform);
        }
    }
}
