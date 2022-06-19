using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    private Animator door_animator;

    public bool IsOpen = false;
    public bool IsClosed = false;

    private bool doorTrigger;

    [SerializeField] private GameObject DoorOpenText;
    [SerializeField] private GameObject DoorCloseText;

    // Start is called before the first frame update
    void Start()
    {
        door_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        DoorCheckTrigger();

    }

    /*this method simply handles door and checks wether there is an intercation with player*/
    private void DoorCheckTrigger()
    {
        if (doorTrigger)
        {
            if (IsOpen)
            {
                DoorOpenText.SetActive(false);
                DoorCloseText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door_animator.SetTrigger("Close");
                    door_animator.SetBool("IsClosed", true);
                    door_animator.SetBool("IsOpen", false);

                    IsClosed = true;
                    IsOpen = false;
                }

            }
            else if (IsClosed)
            {
                DoorOpenText.SetActive(true);
                DoorCloseText.SetActive(false);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door_animator.SetTrigger("Open");
                    door_animator.SetBool("IsOpen", true);
                    door_animator.SetBool("IsClosed", false);

                    IsClosed = false;
                    IsOpen = true;
                }

            }
        }
        else
        {
            DoorCloseText.SetActive(false);
            DoorOpenText.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            doorTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            doorTrigger = false;
        }
    }
}


/*legacy code */
// if (Input.GetKeyDown(KeyCode.E))
// {
//     if (IsOpen)
//     {
//         /*case where door open we close it after 'e' is pressed again*/
//         door_animator.SetTrigger("Close");
//         door_animator.SetBool("IsClosed", true);
//         door_animator.SetBool("IsOpen", false);

//         IsClosed = true;
//         IsOpen = false;
//     }
//     else if (IsClosed)
//     {
//         /*case where door is closed we open it after 'e' is pressed again*/
//         door_animator.SetTrigger("Open");
//         door_animator.SetBool("IsOpen", true);
//         door_animator.SetBool("IsClosed", false);

//         IsClosed = false;
//         IsOpen = true;
//     }
// }