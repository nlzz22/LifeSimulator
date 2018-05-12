using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownAttrType : MonoBehaviour {
    public GameObject dropdownDiscreteAttrPrefab;
    public GameObject continuousAttrPrefab;

    private Dropdown selfDropdown;
    private Transform DropdownAttrAttachment;

	// Use this for initialization
	void Start () {
        selfDropdown = GetComponent<Dropdown>();
        DropdownAttrAttachment = selfDropdown.transform.Find("DropdownAttrAttachment");
    }
	
    public void UpdateValueChanged()
    {
        if (DropdownAttrAttachment == null)
        {
            Start();
        }

        // remove all children from attachment first.
        foreach (Transform child in DropdownAttrAttachment)
        {
            Destroy(child.gameObject);
        }

        if (selfDropdown.value == 1) // discrete
        {
            Instantiate(dropdownDiscreteAttrPrefab, DropdownAttrAttachment);
        } else if (selfDropdown.value == 2) // continuous
        {
            Instantiate(continuousAttrPrefab, DropdownAttrAttachment);
        }
    }

    
}
