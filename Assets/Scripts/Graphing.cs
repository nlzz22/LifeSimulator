using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphing : MonoBehaviour {
    // Finds the equation of a line, given 2 points.
    // Returns false for invalid points.
	public static bool FindLineEquation(float x1, float y1, float x2, float y2, out float gradient, out float constant)
    {
        float numerator = y2 - y1;
        float denominator = x2 - x1;
        if (denominator == 0)
        {
            // dummy values.
            gradient = 0;
            constant = 0;
            return false;
        }

        gradient = numerator / denominator;
        constant = y1 - gradient * x1;
        return true;
    }
}
