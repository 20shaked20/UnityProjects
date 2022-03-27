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
    [Space]

    private Vector2 PlayerMouseInput;
    private Animator animator;
    private CharacterController characterContoller;
    private float yspeed;
    private float originalStepOffset;
    private bool isJumping;
    private bool isGrounded;
    private float lastGroundedTime;
    private float jumpButtonPressedTime;
    private float xRotation;
     

    private int coins_collected;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterContoller = GetComponent<CharacterController>();
        originalStepOffset = characterContoller.stepOffset;

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

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude  /= 2 ;
        }

        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(PlayerCamera.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        yspeed += Physics.gravity.y * Time.deltaTime;

        if(characterContoller.isGrounded) 
        {
            lastGroundedTime = Time.time;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if(Time.time - lastGroundedTime <= jumpButtonGracePeriod) 
        {
            characterContoller.stepOffset = originalStepOffset;
            yspeed = -0.5f;
            animator.SetBool("IsGrounded",true);
            isGrounded = false;
            animator.SetBool("IsJumping",false);
            isJumping = false;
            // animator.setBool("IsFalling",false);

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
