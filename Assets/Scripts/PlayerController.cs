using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController uniqueInstance { get; private set; }

    [SerializeField] private int moveSpeed = 8;
    [SerializeField] private int jumpSpeed = 20;
    [SerializeField] private int rotateSpeed = 10;
    [SerializeField] private int gravityMult = 5;
    public Animator playerAnimator;
    private CharacterController charController;
    private CameraController camController;
    private Vector3 movementVector;
    private float ySpeed;
    private float auxSpeed;
    private bool isJumping; // Added variable to track jump state

    // Start is called before the first frame update
    void Start()
    {
        camController = FindObjectOfType<CameraController>();
    }

    private void Awake()
    {
        uniqueInstance = this;
        charController = GetComponent<CharacterController>();
        isJumping = false; // Initialize isJumping to false
    }

    // Update is called once per frame
    void Update()
    {
        ySpeed = movementVector.y;
        movementVector = camController.transform.forward * Input.GetAxisRaw("Vertical") + camController.transform.right * Input.GetAxisRaw("Horizontal");
        movementVector.y = 0;
        movementVector = movementVector.normalized;

        if (movementVector != Vector3.zero)
        {
            Quaternion aux = Quaternion.LookRotation(movementVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, aux, rotateSpeed * Time.deltaTime);
        }

        movementVector.y = ySpeed;

        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            isJumping = true;
            movementVector.y = jumpSpeed;
        }

        charController.Move(Time.deltaTime * new Vector3(movementVector.x * moveSpeed, movementVector.y, movementVector.z * moveSpeed));
        auxSpeed = new Vector3(movementVector.x, 0, movementVector.z).magnitude * moveSpeed;
        playerAnimator.SetFloat("Speed", auxSpeed);
        playerAnimator.SetFloat("yVel", movementVector.y);
        playerAnimator.SetBool("Jump", isJumping);
    }

    void FixedUpdate()
    {
        if (!charController.isGrounded)
        {
            movementVector.y += Physics.gravity.y * gravityMult * Time.deltaTime;
        }
        else
        {
            movementVector.y = Physics.gravity.y * gravityMult * Time.deltaTime;
            isJumping = false;
        }
    }
}
