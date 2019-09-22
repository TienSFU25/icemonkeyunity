using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers: MonoBehaviour
{
    private static float scale;
    public GameObject thePlane;
    public static float puckHeight = 0.01f;

    void Start()
    {
        scale = thePlane.GetComponent<Renderer>().bounds.size.x / 2;
    }

    public static Vector3 toWorldCoordinates(float x, float y)
    {
        return new Vector3(scale * x, puckHeight, scale * y);
    }
}
