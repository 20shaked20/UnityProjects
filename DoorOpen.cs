using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{   
    private Animator door_animator;
    public bool IsOpen = false;
    public bool IsClosed = false;
    // Start is called before the first frame update
    void Start()
    {
        door_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (IsOpen)
            {
                /*case where door open we close it after 'e' is pressed again*/
                door_animator.SetTrigger("Close");
                door_animator.SetBool("IsClosed", true);
                door_animator.SetBool("IsOpen", false);

                IsClosed = true;
                IsOpen = false;
            }
            else if (IsClosed)
            {
                /*case where door is closed we open it after 'e' is pressed again*/
                door_animator.SetTrigger("Open");
                door_animator.SetBool("IsOpen", true);
                door_animator.SetBool("IsClosed", false);

                IsClosed = false;
                IsOpen = true;
            }
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     /*Close & Open Door */
    //     if (other.CompareTag("Player"))
    //     {
    //         Debug.Log(other.name);
    //         if (IsOpen)
    //         {
    //             /*case where door open we close it after 'e' is pressed again*/
    //             door_animator.SetTrigger("Close");
    //             door_animator.SetBool("IsClosed", true);
    //             door_animator.SetBool("IsOpen", false);

    //             IsClosed = true;
    //             IsOpen = false;
    //         }
    //         else if (IsClosed)
    //         {
    //             /*case where door is closed we open it after 'e' is pressed again*/
    //             door_animator.SetTrigger("Open");
    //             door_animator.SetBool("IsOpen", true);
    //             door_animator.SetBool("IsClosed", false);

    //             IsClosed = false;
    //             IsOpen = true;
    //         }
    //     }
    // }
}
