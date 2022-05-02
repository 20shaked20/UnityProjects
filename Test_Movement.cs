using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Movement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;

    private Animator anim_movement;
    private Vector3 moveDirection;

    private CharacterController controller;



    // Start is called before the first frame update
    private void Start()
    {
        anim_movement = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0,0,moveZ);
        if(moveDirection != Vector3.zero && !(Input.GetKey(KeyCode.LeftShift))){
            Walk();
        }
        else if(moveDirection !=Vector3.zero && Input.GetKey(KeyCode.LeftShift)){
            Run();
        }
        else if(moveDirection == Vector3.zero){
            Idle();
        }

        moveDirection *= moveSpeed;

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim_movement.SetFloat("Speed",0f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        anim_movement.SetFloat("Speed",0.5f, 0.1f, Time.deltaTime);
        moveSpeed = runSpeed;
    }

    private void Idle()
    {
        anim_movement.SetFloat("Speed",1f, 0.1f, Time.deltaTime);
    }
}
