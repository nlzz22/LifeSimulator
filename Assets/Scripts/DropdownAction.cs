using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownAction : MonoBehaviour
{
    private Dropdown dropdownself;
    private GameObject actionAttachment;

    [SerializeField]
    private GameObject attributeActionPrefab;
    [SerializeField]
    private GameObject statusActionPrefab;

    // to correspond with options at the dropdown object.
    private static int ATTRIBUTE = 1;
    private static int STATUS = 2;

    private void Start()
    {
        dropdownself = GetComponent<Dropdown>();
        actionAttachment = dropdownself.gameObject.transform.Find("ActionAttachment").gameObject;
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
        int selectedValue = dropdownself.value;
        if (selectedValue == ATTRIBUTE)
        {
            RemoveAllChild(actionAttachment);
            Instantiate(attributeActionPrefab, actionAttachment.transform);
        }
        else if (selectedValue == STATUS)
        {
            RemoveAllChild(actionAttachment);
            // TODO
            //Instantiate(statusActionPrefab, actionAttachment.transform);
        }
    }
}
