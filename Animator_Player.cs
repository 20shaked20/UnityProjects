using Cinemachine;
using UnityEngine;

public class Animator_Player : MonoBehaviour
{   
    [SerializeField] Transform PlayerCamera;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpButtonGracePeriod;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHorizontalSpeed;
    [SerializeField] private float Sensitivty;
<<<<<<< HEAD
    [SerializeField] private float RunSpeed;
    [Space]

    private Vector2 PlayerMouseInput;
    private Vector3 movementDirection;
=======
    [Space]

    private Vector2 PlayerMouseInput;
>>>>>>> 68362ab21342c6f1e115646b6f61fedd967d22f5
    private Animator animator;
    private CharacterController characterContoller;
    private float yspeed;
    private float originalStepOffset;
    private bool isJumping;
    private bool isGrounded;
<<<<<<< HEAD
    private bool isFalling;
    private bool isRunning;
    private bool isAttacking;
    private float lastGroundedTime;
    private float jumpButtonPressedTime;
    private float xRotation;
=======
    private float lastGroundedTime;
    private float jumpButtonPressedTime;
    private float xRotation;
     
>>>>>>> 68362ab21342c6f1e115646b6f61fedd967d22f5

    private int coins_collected;

    // Start is called before the first frame update
    void Start()
<<<<<<< HEAD
    {   
        animator = GetComponent<Animator>();
        characterContoller = GetComponent<CharacterController>();

        originalStepOffset = characterContoller.stepOffset;
=======
    {
        animator = GetComponent<Animator>();
        characterContoller = GetComponent<CharacterController>();
        originalStepOffset = characterContoller.stepOffset;

>>>>>>> 68362ab21342c6f1e115646b6f61fedd967d22f5
    }

    private void MovePlayerCamera()
    {
        
        xRotation -= PlayerMouseInput.y * Sensitivty;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivty, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

<<<<<<< HEAD
        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

=======
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude  /= 2 ;
        }
>>>>>>> 68362ab21342c6f1e115646b6f61fedd967d22f5

        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(PlayerCamera.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        yspeed += Physics.gravity.y * Time.deltaTime;

        if(characterContoller.isGrounded) 
        {
            lastGroundedTime = Time.time;
        }
<<<<<<< HEAD
        /*Run check*/
        IsRunning();
        
        /*Jump Check*/
        Jump();

        /*Move Checkers*/
        Forward();
        Backwards();
        Left();
        Right();

        /*Attacks*/
        IsAttacking();
        
        if(isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = yspeed;

            characterContoller.Move(velocity * Time.deltaTime);
        }
       
        MovePlayerCamera();
    
}

    private void Forward()
    {
        if(movementDirection != Vector3.zero && Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Forward", true);
        }
        else
        {
            animator.SetBool("Forward",false);
        }
    }

    private void Backwards()
    {
        if(movementDirection != Vector3.zero && Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Backwards", true);
        }
        else
        {
            animator.SetBool("Backwards",false);
        }
    }

    private void Left()
    {
        if(movementDirection != Vector3.zero && Input.GetKey(KeyCode.A))
        {
            animator.SetBool("Left",true);
        }
        else
        {
            animator.SetBool("Left",false);
        }
    }
    
    private void Right()
    {
        if(movementDirection != Vector3.zero && Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Right",true);
        }
        else
        {
            animator.SetBool("Right",false);
        }
    }

    private void IsRunning()
    {
        if(movementDirection !=Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            // inputMagnitude  /= 2 ;
            animator.SetBool("IsRunning",true);
            // isRunning = true;
            // animator.SetBool("IsMoving",false);
            // isMoving = false;
            Vector3 velocity = characterContoller.velocity * RunSpeed;

            characterContoller.SimpleMove(velocity);

        }else
        {
            animator.SetBool("IsRunning",false);
            // isRunning = false;
            // animator.SetBool("IsMoving",true);
            // isMoving = true;
        }
    }

    private void Jump()
    {
=======

>>>>>>> 68362ab21342c6f1e115646b6f61fedd967d22f5
        if(Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if(Time.time - lastGroundedTime <= jumpButtonGracePeriod) 
        {
            characterContoller.stepOffset = originalStepOffset;
            yspeed = -0.5f;
            animator.SetBool("IsGrounded",true);
<<<<<<< HEAD
            isGrounded = true ;
            animator.SetBool("IsJumping",false);
            isJumping = false;
            animator.SetBool("IsFalling",false);
=======
            isGrounded = false;
            animator.SetBool("IsJumping",false);
            isJumping = false;
            // animator.setBool("IsFalling",false);
>>>>>>> 68362ab21342c6f1e115646b6f61fedd967d22f5

            if(Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod) 
            {
                yspeed = jumpSpeed;
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = 0;
                lastGroundedTime = 0;
            }
        }else
        {
            characterContoller.stepOffset = 0;
            animator.SetBool("IsGrounded",false);
            isGrounded = false;

<<<<<<< HEAD
            if((isJumping && yspeed < 0) || yspeed < -2) 
            {
                animator.SetBool("IsFalling",true);
            }
        }
    }

    private void IsAttacking()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool("IsAttacking",true);
            // isAttacking = true;

        }else
        {
            animator.SetBool("IsAttacking", false);
            // isAttacking = false;
            
        }
    }

=======
            // if((isJumping && yspeed < 0) || yspeed < -2) 
            // {
            //     animator.SetBool("IsFalling",true);
            // }
        }

        if(movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }else
        {
            animator.SetBool("IsMoving",false);
        }

        if(isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = yspeed;

            characterContoller.Move(velocity * Time.deltaTime);
        }
        MovePlayerCamera();
    }

/* This is camera movement for normal camera ( not cinemachine ) */
    // void FixedUpdate()
    // {
        //  // Converting the mouse position to a point in 3D-space
        // Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, sensitivity));
        // // Using some math to calculate the point of intersection between the line going through the camera and the mouse position with the XZ-Plane
        // float t = cam.transform.position.y / (cam.transform.position.y - point.y);
        // Vector3 finalPoint = new Vector3(t * (point.x - cam.transform.position.x) + cam.transform.position.x, sensitivity, t * (point.z - cam.transform.position.z) + cam.transform.position.z);
        // //Rotating the object to that point
        // transform.LookAt(finalPoint, Vector3.up);
    // }
>>>>>>> 68362ab21342c6f1e115646b6f61fedd967d22f5

    private void OnAnimatorMove()
    {
        if(isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = yspeed * Time.deltaTime;

            characterContoller.Move(velocity);  
        }
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
