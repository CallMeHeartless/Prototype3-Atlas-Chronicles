using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            if (gameObject.CompareTag("PrimaryPickUp"))
            {
                GameStats.MapsBoard[GameStats.LevelLoctation]++;
            }
            else
            {
                if (gameObject.CompareTag("SecondayPickUp"))
                {
                    GameStats.NoteBoard[GameStats.LevelLoctation]++;
                }
            }
            gameObject.SetActive(false);
        }
    }
}
