﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telescope : MonoBehaviour
{
    // Start is called before the first frame update
    int POVNormal = 60;//standed POV
    public float newPOV =0;
    bool looking = true;
    public float delay= 2;
    public float loading = 2;
    public GameObject camera;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delay <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UIInfo();
            }
            else
            {
                delay -= Time.deltaTime;
            }


        }
        void updateNewView()
        {
            if (looking)
            {
                StartCoroutine("zoom");  //gameObject.GetComponent<Camera>().fieldOfView = newPOV;
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                camera.GetComponent<Camera>().fieldOfView = POVNormal;
            }
            looking = !looking;
            loading = 2;


        }

        void UIInfo()
        {
            if (delay <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (looking)
                    {
                        camera.GetComponent<Camera>().fieldOfView = newPOV;
                        transform.GetChild(0).gameObject.SetActive(true);
                        transform.GetChild(1).gameObject.SetActive(false);
                    }
                    else
                    {
                        camera.GetComponent<Camera>().fieldOfView = POVNormal;
                        transform.GetChild(0).gameObject.SetActive(false);
                        transform.GetChild(1).gameObject.SetActive(true);
                    }
                    looking = !looking;
                    delay = 2;
                }
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }

    IEnumerator zoom()
    {

        //play animtion

        yield return new WaitForSeconds(.5f);
        transform.GetChild(0).gameObject.SetActive(true);
        camera.GetComponent<Camera>().fieldOfView = newPOV;
    }
}
