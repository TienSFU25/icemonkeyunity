using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject arrowBase;
    public GameObject arrowHead;

    private float arrowUnitLength;
    private float arrowBaseLength;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pv = new Vector3(0, 0, 0);

        //GameObject ab = Instantiate(arrowBase, pv, Quaternion.identity);

        // 1 scale how much size?
        arrowBaseLength = arrowBase.GetComponent<Renderer>().bounds.size.x;
        arrowUnitLength = (1 / arrowBase.transform.localScale.x) * arrowBaseLength;
        //arrowUnitLength = (1 / ab.transform.localScale.x) * ab.GetComponent<Renderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject MakeArrow(Vector3 from, Vector3 to)
    {
        Vector3 shiftedFrom = new Vector3(from.x + arrowBaseLength / 2, from.y, from.z);
        Vector3

        GameObject arrow = Instantiate(arrowBase, shiftedFrom, Quaternion.identity);
        //return new ();
        return null;
    }
}
