using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditEventFunction : MonoBehaviour {
    private GameObject currentEventFunctionWhole;
    private GameObject functionsGrid;

    private GameObject FindCurrentEventFunctionWhole()
    {
        if (currentEventFunctionWhole == null)
        {
            currentEventFunctionWhole = transform.parent.parent.gameObject;
        }

        return currentEventFunctionWhole;
    }

    private GameObject FindFunctionsGrid()
    {
        if (functionsGrid == null)
        {
            GameObject currEventFuncWhole = FindCurrentEventFunctionWhole();
            functionsGrid = currEventFuncWhole.transform.parent.gameObject;
        }

        return functionsGrid;
    }

	public void Edit()
    {
        FindFunctionsGrid();

        // disable all event function whole.
        int numChild = functionsGrid.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            functionsGrid.transform.GetChild(i).gameObject.SetActive(false);
        }
        // enable only the current event function whole.
        currentEventFunctionWhole.SetActive(true);

        // swap over the view from rep to input view.
        transform.parent.gameObject.SetActive(false); // hide rep.
        currentEventFunctionWhole.transform.Find("EventFunctionInput").gameObject.SetActive(true); // show input.

    }
}
