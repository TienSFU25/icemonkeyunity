using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Trajectories = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<TimeState>>;

public class TimeState
{
    public float X;
    public float Y;
    public float T;
    public string S;
}

public class Main : MonoBehaviour
{
    public GameObject thePuck;
    public GameObject thePlane;
    private GameObject pucky;
    private Rigidbody rb;

    private List<TimeState> timeStates;
    private int index = 1;

    private float timeElapsed = 1;

    // Start is called before the first frame update
    void Start()
    {
        string puckPath = Application.dataPath + "/JS/puck.json";
        string puckData = File.ReadAllText(puckPath);
        Trajectories trajectories = JsonConvert.DeserializeObject<Trajectories>(puckData);
        timeStates = trajectories["302"];

        Vector3 originalPosition = Helpers.toWorldCoordinates(timeStates[0].X, timeStates[0].Y);
        pucky = Instantiate(thePuck, originalPosition, Quaternion.identity);
        rb = pucky.GetComponent<Rigidbody>();

        StartCoroutine(RunShit());

        Vector3 pv = Helpers.toWorldCoordinates(-1, 0);
        Instantiate(thePuck, Helpers.toWorldCoordinates(-1, -1), Quaternion.identity);
        Instantiate(thePuck, Helpers.toWorldCoordinates(-1, 1), Quaternion.identity);
        Instantiate(thePuck, Helpers.toWorldCoordinates(1, -1), Quaternion.identity);
        Instantiate(thePuck, Helpers.toWorldCoordinates(1, 1), Quaternion.identity);
        //Instantiate(thePuck, toWorldCoordinates(1, 0), Quaternion.identity);
        //Instantiate(thePuck, toWorldCoordinates(-1, 0), Quaternion.identity);

        //StartCoroutine(ExpandToPosition(ab, lTemp, 3));
        Arrow arrowMaker = GetComponent<Arrow>();
        Vector3 from = Helpers.toWorldCoordinates(-1, 0);
        Vector3 to = Helpers.toWorldCoordinates(1, 0);

        arrowMaker.MakeArrow(from, to);

        //GameObject a = Arrow.MakeArrow();
    }

    // only fudges around in X direction
    public IEnumerator ExpandToPosition(GameObject go, Vector3 to, float t)
    {
        Transform thingToExpand = go.transform;
        var currentScale = thingToExpand.localScale;
        var elapsed = 0f;

        float currentXLength = go.GetComponent<Renderer>().bounds.size.x;
        float targetLength = Vector3.Distance(go.transform.position, to) + currentXLength / 2;
        float scaleTarget = targetLength / arrowUnitLength;
        Vector3 scaleTo = go.transform.localScale;
        scaleTo.x = scaleTarget;

        while (elapsed < 1)
        {
            elapsed += Time.deltaTime / t;

            Vector3 dest = Vector3.Lerp(currentScale, scaleTo, elapsed);
            float dx = (dest - thingToExpand.localScale).x * arrowUnitLength / 2;
            Vector3 shiftedPos = thingToExpand.transform.position;
            shiftedPos.x += dx;

            thingToExpand.transform.position = shiftedPos;
            thingToExpand.localScale = dest;

            yield return null;
        }
    }

    IEnumerator RunShit()
    {
        if (index == timeStates.Count)
        {
            yield return null;
        }

        Vector3 nextPosition = Helpers.toWorldCoordinates(timeStates[index].X, timeStates[index].Y);

        float currentTime = timeStates[index - 1].T;
        float timeToMove = timeStates[index].T - currentTime;
        //Debug.Log("moving to " + scale * timeStates[index].X + ", " + scale * timeStates[index].Y + " over " + timeToMove + " seconds");

        IEnumerator co = MoveToPosition(rb.transform, nextPosition, timeToMove);

        while (co.MoveNext())
        {
            IEnumerator result = co.Current as IEnumerator;
            //Debug.Log(result);
            yield return result;
        }

        index += 1;
        yield return RunShit();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("clgt");
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        timeElapsed = 0f;

        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, timeElapsed);
            yield return "foo";
        }
    }
}
