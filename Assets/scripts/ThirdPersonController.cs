using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    // All your public variables (velocity, jumpForce, etc.) remain the same.
    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 3.5f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;
    [Tooltip("Key to toggle the cursor on/off.")]
    public KeyCode toggleKey = KeyCode.M;
    [Tooltip("Start with cursor visible?")]
    public bool cursorVisibleAtStart = false;

    // Private variables remain the same
    float jumpElapsedTime = 0;
    bool isJumping = false;
    bool isSprinting = false;
    bool isCrouching = false;
    bool wasAirborne = false;
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputCrouch;
    bool inputSprint;

    Animator animator;
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetCursor(cursorVisibleAtStart);
    }

    void Update()
    {
        // --- NEW: Check if the character is currently in the landing animation state ---
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
        {
            // If we are landing, reset movement inputs and do nothing else this frame.
            inputHorizontal = 0;
            inputVertical = 0;
            return; // This line stops the rest of the Update function from running
        }

        // The rest of the Update function remains the same
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputJump = Input.GetAxis("Jump") == 1f;
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        if (inputCrouch)
            isCrouching = !isCrouching;

        if (cc.isGrounded && animator != null)
        {
            animator.SetBool("crouch", isCrouching);
            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed);
            isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
            animator.SetBool("sprint", isSprinting);
        }

        if (animator != null)
            animator.SetBool("air", !cc.isGrounded);

        if (inputJump && cc.isGrounded)
        {
            isJumping = true;
        }

        if (wasAirborne && cc.isGrounded)
        {
            animator.SetTrigger("land");
        }
        wasAirborne = !cc.isGrounded;

        HeadHittingDetect();

        if (Input.GetKeyDown(toggleKey))
        {
            bool currentlyVisible = Cursor.visible;
            SetCursor(!currentlyVisible);
        }
    }

    // FixedUpdate remains exactly the same as before
    private void FixedUpdate()
    {
        float velocityAdittion = 0;
        if (isSprinting)
            velocityAdittion = sprintAdittion;
        if (isCrouching)
            velocityAdittion = -(velocity * 0.50f);

        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;

        if (isJumping)
        {
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        directionY = directionY - gravity * Time.deltaTime;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        forward = forward * directionZ;
        right = right * directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;
        cc.Move(verticalDirection + horizontalDirection);
    }
    
    // The other helper methods remain the same
    void HeadHittingDetect()
    {
        if (!isJumping) return;
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;
        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }

    void SetCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}