using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Rigidbody PlayerBody;
    [Space] 
    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivty;
    [SerializeField] private float JumpForce;

    private bool JumpKeyWasPressed;
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRotation;

    private int coins_collected;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if space key is pressed down.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpKeyWasPressed = true;
        }
        
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayerCamera();
    }
    // is called once every physic update
    private void FixedUpdate(){
        
        // movement
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        // collision fix.
        if(Physics.OverlapSphere(groundCheckTransform.position , 0.1f, playerMask).Length == 0)
        {
            return; 
        }

        // jumps 
        if (JumpKeyWasPressed)
        {
            PlayerBody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
            JumpKeyWasPressed = false;
        }
    }

    private void MovePlayerCamera(){
        
        xRotation -= PlayerMouseInput.y * Sensitivty;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivty, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    // coin collector method
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            coins_collected++;
        }    
    }
}
