using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone : MonoBehaviour
{
    public LineRenderer line;
    private bool ground = false;
    private Vector3 goal;
    public int nextPoint = 2;
    public float speed;
    public Vector3[] dummy;

    // Start is called before the first frame update
    void Start()
    {
        //dummy=new Vector3[gameObject.GetComponentInParent<stoneArc>().size];
       // gameObject.GetComponentInParent<LineRenderer>().GetPositions(dummy);
        //gameObject.GetComponent<LineRenderer>().SetPositions(dummy);
       //line = gameObject.GetComponent<LineRenderer>();
        goal = line.GetPosition(nextPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ground)
        {
            if (Vector3.Distance(transform.position, goal) <= 0.2f)//at loctation
            {
                nextPoint++;
                
                if (nextPoint != gameObject.GetComponentInParent<stoneArc>().size)
                {
                    goal = line.GetPosition(nextPoint);

                }
                else
                {
                    ground = true;
                }

            }
            transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);

        }
       
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1))
            {
                ground = true;
            }
        
    }
}
