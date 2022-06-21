using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSit : MonoBehaviour
{

    private bool sit_trigger = false;

    private bool is_sitting = false;

    private GameObject player_object;

    // Update is called once per frame
    void Update()
    {
        if (sit_trigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!is_sitting)
                {

                    print("Character is now sitting");
                    player_object.GetComponent<Animator>().SetTrigger("Sit");

                    is_sitting = true;
                }
                else
                {
                    print("Character is now standing");
                    player_object.GetComponent<Animator>().SetTrigger("Stand");

                    is_sitting = false;
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sit_trigger = true;
            player_object = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            sit_trigger = false;
            player_object = null;
        }
    }
}
