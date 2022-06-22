using Cinemachine;
using UnityEngine;

public class Animator_Player : MonoBehaviour
{   
    /*visual*/
    [SerializeField] Transform PlayerCamera;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpButtonGracePeriod;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHorizontalSpeed;
    [SerializeField] private float Sensitivty;
    [SerializeField] private float RunSpeed;

    [SerializeField] private InventoryObject inventory;
    
    [Space]

    /*movement*/
    private Vector2 PlayerMouseInput;
    private Vector3 movementDirection;
    private Animator animator;
    private CharacterController characterContoller;
    private float yspeed;
    private float originalStepOffset;
    private bool isJumping;
    private bool isGrounded;
    private bool isFalling;
    private bool isRunning;
    private bool isCrouching;
    private bool isAttacking;
    private float lastGroundedTime;
    private float jumpButtonPressedTime;
    private float xRotation;

    /*npc handle */
    private GameObject npc_object;
    private bool npc_trigger;

    [SerializeField] private GameObject npcInteract;

    /*temp fix for jump when starting the game*/
    private bool first_time = false; 

    /*item collection trigger*/
    private int coins_collected;
    
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

        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);


        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(PlayerCamera.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        yspeed += Physics.gravity.y * Time.deltaTime;

        if(characterContoller.isGrounded) 
        {
            lastGroundedTime = Time.time;
        }
        /*Run check*/
        IsRunning();

        /*Crouch check*/
        IsCrouching();
        
        /*Jump Check*/
        Jump();

        /*Move Checkers*/
        Forward();
        Backwards();
        Left();
        Right();

        /*Attacks*/
        IsAttacking();

        /*Interaction*/

        if(npc_trigger)
        {
            npcInteract.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                print("Hello traveller!");
            }
        }
        else
        {
            npcInteract.SetActive(false);
        }
        
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
            animator.SetBool("IsRunning",true);
            Vector3 velocity = characterContoller.velocity * RunSpeed;

            characterContoller.SimpleMove(velocity);

        }else
        {
            animator.SetBool("IsRunning",false);
        }
    }

    private void Jump()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            jumpButtonPressedTime = Time.time;
            first_time = true;
        }

        if(Time.time - lastGroundedTime <= jumpButtonGracePeriod) 
        {
            characterContoller.stepOffset = originalStepOffset;
            yspeed = -0.5f;
            animator.SetBool("IsGrounded",true);
            isGrounded = true;
            animator.SetBool("IsJumping",false);
            isJumping = false;
            animator.SetBool("IsFalling",false);

            if((Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod) && first_time) 
            {
                yspeed = jumpSpeed;
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = 0;
                lastGroundedTime = 0;
            }
        }
        else
        {
            characterContoller.stepOffset = 0;
            animator.SetBool("IsGrounded",false);
            isGrounded = false;

            if((isJumping && yspeed < 0) || yspeed < -2) 
            {
                animator.SetBool("IsFalling",true);
            }
        }
    }

    private void IsCrouching(){
        
        if(Input.GetKey(KeyCode.C))
        {   
            if(isCrouching == false){
                animator.SetBool("IsCrouching",true);
                isCrouching = true;
                // characterContoller.height = 1.8f;
            }else{
                animator.SetBool("IsCrouching", false);
                isCrouching = false;
                // characterContoller.height = 1.25f;

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
        /*object pickup*/
        var item = other.GetComponent<Item>();
        if(item)
        {
            inventory.AddItem(item.item,1);
            Destroy(other.gameObject);
        }

        /*npc interaction*/
        if(other.tag == "Enemy")
        {
            npc_trigger = true;
            npc_object = other.gameObject;
        }  
    }

    private void OnTriggerExit(Collider other)
    {   
        /*if we done with npc*/
        if(other.tag == "Enemy")
        {
            npc_trigger = false;
            npc_object = null;
        }
    }

    private void OnApplicationQuit()
    {   
        /*this will remove all items in inventory when game quits, will remove later*/
        // inventory.Container.Clear();

    }
}
