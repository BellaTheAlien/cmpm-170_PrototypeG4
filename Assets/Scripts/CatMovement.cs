using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CatMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 6f;
    public float sneakSpeed = 1.2f;
    public float climbSpeed = 2f;

    [Header("Jump Settings")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.8f;

    [Header("Detection")]
    public float climbCheckDistance = 0.7f;
    public LayerMask climbableLayer;

    private CharacterController controller;
    private Animator anim;
    private Vector3 velocity;
    private bool isClimbing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isClimbing)
        {
            HandleClimb();
            return;
        }

        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    // Movement
    // Controls:
    //   W A S D  = Move
    //   Left Shift = Run
    //   Left Control = Sneak
    //   Move toward climbable surface + Space = Start climbing
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0, v);

        if (input.magnitude > 1f)
            input.Normalize();

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed = runSpeed;
        else if (Input.GetKey(KeyCode.LeftControl))
            speed = sneakSpeed;

        Vector3 move = transform.TransformDirection(input) * speed;
        controller.Move(move * Time.deltaTime);

        anim.SetFloat("Speed", move.magnitude);
        anim.SetBool("Sneaking", Input.GetKey(KeyCode.LeftControl));
        anim.SetBool("Running", Input.GetKey(KeyCode.LeftShift));

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, climbCheckDistance, climbableLayer))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                isClimbing = true;
                velocity = Vector3.zero;
                anim.SetBool("Climbing", true);
            }
        }
    }

    // Jumping
    // Controls:
    //   Space = Jump
    void HandleJump()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("Jump");
        }
    }

    // Climbing
    // Controls:
    //   Move into climbable surface + Space to begin
    //   W / S = Climb up / down
    //   Release Space to stop climbing
    void HandleClimb()
    {
        float v = Input.GetAxis("Vertical");
        Vector3 climbMove = new Vector3(0, v * climbSpeed, 0);

        controller.Move(climbMove * Time.deltaTime);

        anim.SetFloat("ClimbSpeed", v);

        if (!Input.GetKey(KeyCode.Space))
        {
            isClimbing = false;
            anim.SetBool("Climbing", false);
        }
    }

    // Gravity
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
