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
    public GameObject player;
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
            newStone.transform.parent = gameObject.transform;
            newStone.transform.position = lineRend.GetPosition(1);
            newStone.GetComponent<stone>().speed = velcoity;

            newStone.GetComponent<stone>().dummy = new Vector3[size];
            lineRend.GetPositions(newStone.GetComponent<stone>().dummy);

            newStone.GetComponent<LineRenderer>().positionCount = size;
            newStone.GetComponent<LineRenderer>().SetPositions(newStone.GetComponent<stone>().dummy);

            //newStone.GetComponent<Rigidbody>().AddForce(, ForceMode.Force);
            //new Vector3(,,);
            //float rad = Mathf.Deg2Rad * anglechange;
            //newStone.GetComponent<Rigidbody>().velocity = (new Vector3(((-velcoity * rad) + velcoity), velcoity * rad, 0));
        }
    }

    void mathing()
    {
        Vector3[] newLoctation = new Vector3[size];
        newLoctation[0] = player.transform.position;
        for (int i = 1; i < size; i++)
        {
            //newLoctation[i] = new Vector3(i * velcoity, ()+2, 0);
            //float a = velcoity * velcoity * Mathf.Sin(2 * (Mathf.Deg2Rad * angle)) / 9.81f;
            newLoctation[i] = point((float)i/ (float)size );
            newLoctation[i] += player.transform.position;
        }
       

        lineRend.positionCount = newLoctation.Length;
        lineRend.SetPositions(newLoctation);
    }
    Vector3 point(float i )
    {
        Vector3 currentPoint;
        float rad = Mathf.Deg2Rad * angle;
        currentPoint.x = i * maxVel;
        currentPoint.y = currentPoint.x * Mathf.Tan(rad) - ((9.81f * currentPoint.x * currentPoint.x) / (2 * velcoity * velcoity * Mathf.Cos(rad) * Mathf.Cos(rad)));
        currentPoint.z = 0;
        //rotate
        if (rotation != 0)
        {
            
            float tempx, tempz, radrot;
            radrot = Mathf.Deg2Rad * rotation;
            tempx = currentPoint.x * Mathf.Cos(radrot) - currentPoint.z * Mathf.Sin(radrot);
            tempz = currentPoint.x * Mathf.Sin(radrot) + currentPoint.z * Mathf.Cos(radrot);

            currentPoint.z = tempz;
            currentPoint.x = tempx;
        }
       
        

        return currentPoint;
    }

    Vector3 dummy()
    {
        Vector3 currentPoint;
        float rad = Mathf.Deg2Rad * angle;
        currentPoint.x = size * maxVel;
        currentPoint.y = currentPoint.x * Mathf.Tan(rad) - ((9.81f * currentPoint.x * currentPoint.x) / (2 * velcoity * velcoity * Mathf.Cos(rad) * Mathf.Cos(rad)));
        currentPoint.z = 0;
        //rotate
        if (rotation != 0)
        {

            float tempx, tempz, radrot;
            radrot = Mathf.Deg2Rad * rotation;
            tempx = currentPoint.x * Mathf.Cos(radrot) - currentPoint.z * Mathf.Sin(radrot);
            tempz = currentPoint.x * Mathf.Sin(radrot) + currentPoint.z * Mathf.Cos(radrot);

            currentPoint.z = tempz;
            currentPoint.x = tempx;
        }
        return currentPoint;
        


    }
}
