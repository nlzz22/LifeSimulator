using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    private float positionx;
    private float positiony;

    public void SavePosition(float x, float y)
    {
        positionx = x;
        positiony = y;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(positionx, positiony, 0);
    }
}