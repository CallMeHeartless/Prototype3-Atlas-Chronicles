using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class stoneArc : MonoBehaviour
{
    public LineRenderer lineRend;
    public GameObject stone;
    public float angle;
    public float velcoity;
    public int size;
    public float increaser;
    public int maxVel;
    public float rotation;
    // Start is called before the first frame update
    void Start()
    {
        //lineRend.GetComponent<LineRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        if ((size != 0)&&(Application.isPlaying))
        {
            mathing();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject newStone = GameObject.Instantiate(stone,transform.position,transform.rotation);
            float anglechange = Mathf.Tan(Mathf.Deg2Rad * angle);
            newStone.GetComponent<Rigidbody>().AddForce(new Vector3(-(velcoity * anglechange) + velcoity * increaser, velcoity * anglechange* increaser, 0), ForceMode.Acceleration);
        }
    }

    void mathing()
    {
        Vector3[] newLoctation = new Vector3[size];
        for (int i = 1; i < size; i++)
        {
            //newLoctation[i] = new Vector3(i * velcoity, ()+2, 0);
            //float a = velcoity * velcoity * Mathf.Sin(2 * (Mathf.Deg2Rad * angle)) / 9.81f;
            newLoctation[i] = point((float)i/ (float)size, maxVel);
        }
       

        lineRend.positionCount = newLoctation.Length;
        lineRend.SetPositions(newLoctation);
    }
    Vector3 point(float i, float max)
    {
        Vector3 currentPoint;
        float rad = Mathf.Deg2Rad * angle;
        currentPoint.x = i * max;
        currentPoint.y = currentPoint.x * Mathf.Tan(rad) - ((9.81f * currentPoint.x * currentPoint.x) / (2 * velcoity * velcoity * Mathf.Cos(rad) * Mathf.Cos(rad)));

        //rotate
        if (rotation != 0)
        {
            currentPoint.z = i * max * (rotation / 90);
            currentPoint.x = i * max * (90 / rotation);
        }
        else
        {
            currentPoint.z = 0;
        }
        

        return currentPoint;
    }
}
