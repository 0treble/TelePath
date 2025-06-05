using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip clickSound;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float gravity = 9.81f;

    private float verticalVelocity;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    // UDP movement flag
    private bool hasUDPCommand = false;
    private float commandTimer = 0.5f;
    private float lastCommandTime = 0f;
    private const float commandCooldown = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // Subscribe to UDP messages
        UDPHandler.CommandReceived += HandleUDPCommand;
    }

    private void Update()
    {
        InputManagement();
        Movement();
        CheckForMenu();
    }

    private void Movement()
    {
        GroundMovement();
    }

    private void GroundMovement()
    {
        Vector3 moveDirection = new Vector3(turnInput, 0, moveInput).normalized;

        if (moveDirection.magnitude >= 0.1f || commandTimer > 0)
        {
            animator.SetBool("IsMoving", true);
            RotatePlayer(moveDirection);
            MovePlayer(moveDirection);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        ApplyGravity();

        if (commandTimer > 0)
        {
            commandTimer -= Time.deltaTime;
            if (commandTimer <= 0)
            {
                moveInput = 0;
                turnInput = 0;
            }
        }
    }

    private void RotatePlayer(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
        }
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        Vector3 move = Time.deltaTime * walkSpeed * moveDirection;
        move.y = verticalVelocity * Time.deltaTime;
        controller.Move(move);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    private void InputManagement()
    {
        if (!hasUDPCommand)
        {
            moveInput = Input.GetAxisRaw("Vertical");
            turnInput = Input.GetAxisRaw("Horizontal");
        }
    }

    private void HandleUDPCommand(int command)
    {
        if (Time.time - lastCommandTime < commandCooldown) return;
        lastCommandTime = Time.time;
        hasUDPCommand = true;

        SimulateKeyPress(command);
        commandTimer = 0.5f;
    }

    private void SimulateKeyPress(int command)
    {
        switch (command)
        {
            //turn is X-Axis move is Z-Axis
            case 1: moveInput = 1; break;  // Simulating Up Arrow
            case 2: moveInput = -1; break; // Simulating Down Arrow
            case 3: turnInput = -1; break; // Simulating Left Arrow
            case 4: turnInput = 1; break;  // Simulating Right Arrow
            default: moveInput = 0; turnInput = 0; break;
        }
    }

    private void CheckForMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlaySound(clickSound);
            SceneManager.LoadScene("Main Menu");
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        UDPHandler.CommandReceived -= HandleUDPCommand;
    }
}
