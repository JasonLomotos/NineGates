using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private bool shouldFaceMoveDirection = false;



    private CharacterController Controller;
    private Vector3 moveInput;
    private Vector3 velocity;
    private float originalHeight;
    private bool isCrouching = false;
    private bool isSprinting = false;

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        originalHeight = Controller.height;
        
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
    }

    public void onCrouch(InputAction.CallbackContext context)
    {
        Debug.Log($"Crouch Input: {context.phase}");
        if (context.ReadValue<float>() > 0)
        {
            isCrouching = true;
            Controller.height = crouchHeight;
            Debug.Log("Crouched!");
        }
        else
        {
            isCrouching = false;
            Controller.height = originalHeight;
            Debug.Log("Normal!");
        }
    }
    
    public void onJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jump Input: {context.phase} - is Grounded: {Controller.isGrounded}");
        if (context.performed && Controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Jump!");
        }
    }

    public void onSprint(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            isSprinting = true;
            Debug.Log("Sprinting!");
        }
        else
        {
            isSprinting = false;
            Debug.Log("Stopped Sprinting!");
        }
    }
    

    void Update()
    {
        float currentSpeed = moveSpeed;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isSprinting)
            currentSpeed = sprintSpeed;

        forward.y = 0; // Ignore vertical component for movement direction
        right.y = 0; // Ignore vertical component for movement direction

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        Controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        velocity.y += gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);
        
    }
}
