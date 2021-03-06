using Cinemachine;
using UnityEngine;

public class Animator_Player : MonoBehaviour
{

    /*to be changed later*/
    public MouseItem mouseItem = new MouseItem();

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
    public Vector3 movementDirection;
    private Transform follow_target;

    private Animator animator;
    private CharacterController characterContoller;
    private float yspeed;
    private float originalStepOffset;
    private bool isJumping;
    public bool isGrounded;
    private bool isFalling;
    private bool isRunning;
    private bool isCrouching;
    private bool isAttacking;
    private bool isSwimming;
    private bool isMoving;
    private float lastGroundedTime;
    private float jumpButtonPressedTime;
    private float xRotation;


    /*show inventory*/
    private bool in_inventory = false;
    [SerializeField] private GameObject InventoryWindow;

    /*temp fix for jump when starting the game*/
    private bool first_time = false;

    /*item collection trigger*/
    private int coins_collected;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterContoller = GetComponent<CharacterController>();

        originalStepOffset = characterContoller.stepOffset;

        follow_target = transform.GetChild(2); /* get follow target*/

        /*startups for setting false objects*/
        InventoryWindow.SetActive(false);
    }

    private void MovePlayerCamera()
    {
        /*using right click, will rotate mouse*/
        if (movementDirection != Vector3.zero)
        {
            xRotation -= PlayerMouseInput.y * Sensitivty;
            transform.Rotate(0f, PlayerMouseInput.x * Sensitivty, 0f);
        }
        else
        {
            /*revert camera to look behind player*/
            // PlayerCamera.position = transform.position;
            // PlayerCamera.rotation = transform.rotation;

            //position movement
            PlayerCamera.position = Vector3.Lerp(PlayerCamera.position, follow_target.position, (1f * Time.deltaTime));
            // rotation movement
            PlayerCamera.rotation = Quaternion.Lerp(PlayerCamera.rotation, follow_target.rotation, (1f * Time.deltaTime));
        }

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

        if (characterContoller.isGrounded)
        {
            lastGroundedTime = Time.time;
        }


        /*pop up inv window*/
        if (!in_inventory)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                InventoryWindow.SetActive(true);
                in_inventory = true;
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

            /*Swim*/
            Is_Swimming();

            MovePlayerCamera();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                InventoryWindow.SetActive(false);
                in_inventory = false;
            }
        }

        /*save&load*/
        Save_Load();

        /*in air check*/
        if (isGrounded == false)
        {

            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = yspeed;

            characterContoller.Move(velocity * Time.deltaTime);
        }

        


    }

    private void Save_Load()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            inventory.Load();
        }
    }

    private void Is_Swimming()
    {
        float speed = 1f;
        // if (isSwimming)
        // {
            float translation = Input.GetAxis("Vertical") * speed;
            float straffe = Input.GetAxis("Horizontal") * speed;

            // adjust the speed accordingly. with the modification , it was super fast for me.
            // notice there is no limit on how far you can go :)
            float swimY = Input.GetAxis("Mouse Y") * speed * 2f;
            translation *= Time.deltaTime;
            straffe *= Time.deltaTime;

            transform.Translate(straffe, swimY, translation);
        // }
    }

    private void Forward()
    {
        if (movementDirection != Vector3.zero && Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Forward", true);
            // isMoving = true;
        }
        else
        {
            animator.SetBool("Forward", false);
            // isMoving = false;
        }
    }

    private void Backwards()
    {
        if (movementDirection != Vector3.zero && Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Backwards", true);
            // isMoving = true;
        }
        else
        {
            animator.SetBool("Backwards", false);
            // isMoving = false;
        }
    }

    private void Left()
    {
        if (movementDirection != Vector3.zero && Input.GetKey(KeyCode.A))
        {
            animator.SetBool("Left", true);
            // isMoving = true;
        }
        else
        {
            animator.SetBool("Left", false);
            // isMoving = false;
        }
    }

    private void Right()
    {
        if (movementDirection != Vector3.zero && Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Right", true);
            // isMoving = true;
        }
        else
        {
            animator.SetBool("Right", false);
            // isMoving = false;
        }
    }

    private void IsRunning()
    {
        if (movementDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("IsRunning", true);
            Vector3 velocity = characterContoller.velocity * RunSpeed;

            characterContoller.SimpleMove(velocity);

            // isMoving = true;

        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            jumpButtonPressedTime = Time.time;
            first_time = true;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterContoller.stepOffset = originalStepOffset;
            yspeed = -0.5f;
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("IsFalling", false);

            if ((Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod) && first_time)
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
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            if ((isJumping && yspeed < 0) || yspeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }
    }

    private void IsCrouching()
    {

        if (Input.GetKey(KeyCode.C))
        {
            if (isCrouching == false)
            {
                animator.SetBool("IsCrouching", true);
                isCrouching = true;
                // characterContoller.height = 1.8f;
            }
            else
            {
                animator.SetBool("IsCrouching", false);
                isCrouching = false;
                // characterContoller.height = 1.25f;

            }
        }
    }

    private void IsAttacking()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool("IsAttacking", true);
            // isAttacking = true;

        }
        else
        {
            animator.SetBool("IsAttacking", false);
            // isAttacking = false;

        }
    }


    private void OnAnimatorMove()
    {
        if (isGrounded)
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
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            inventory.AddItem(_item, 1);
            Destroy(other.gameObject);
        }

        /*swimming*/
        if (other.tag == "Water")
        {
            animator.SetTrigger("InWater");
            animator.SetBool("IsSwim", true);
            isSwimming = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        /*swimming done*/
        if (other.tag == "Water")
        {
            animator.SetBool("IsSwim", false);
            isSwimming = false;
        }
    }

    private void OnApplicationQuit()
    {
        /*this will remove all items in inventory when game quits, will remove later*/
        inventory.Container.Items = new InventorySlot[28];

    }
}
