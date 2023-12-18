using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController uniqueInstance { get; private set; }

    [SerializeField] private int moveSpeed = 8;
    [SerializeField] private int jumpSpeed = 20;
    public float bounceSpeed;
    [SerializeField] private int rotateSpeed = 10;
    [SerializeField] private int gravityMult = 5;
    public Animator playerAnimator;
    private CharacterController charController;
    private CameraController camController;
    private Vector3 movementVector;
    private float ySpeed;
    private float auxSpeed;
    private bool isJumping;
    private float idleTimer;
    private int idleThreshold = 10;
    private bool lastGrounded;
    public GameObject jumpParticle;
    public GameObject landingParticle;
    public float invulnerabilityDuration = 1;
    public float invulnerabilityCounter;
    public GameObject[] modelParts;

    // Start is called before the first frame update
    void Start()
    {
        camController = FindObjectOfType<CameraController>();
        lastGrounded = true;
    }

    private void Awake()
    {
        UIController.uniqueInstance.FadeIn();
        uniqueInstance = this;
        charController = GetComponent<CharacterController>();
        isJumping = false;
        idleTimer = 0f;
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
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleThreshold)
            {
                playerAnimator.SetBool("Rest", true);
                idleTimer = 0f;
            }
            else
                playerAnimator.SetBool("Rest", false);


        }

        movementVector.y = ySpeed;

        if (charController.isGrounded)
        {
            jumpParticle.SetActive(false);
            if (!lastGrounded)
                landingParticle.SetActive(true);
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                movementVector.y = jumpSpeed;
                jumpParticle.SetActive(true);
                idleTimer = 0f;
            }
        }
        lastGrounded = charController.isGrounded;
        charController.Move(Time.deltaTime * new Vector3(movementVector.x * moveSpeed, movementVector.y, movementVector.z * moveSpeed));
        auxSpeed = new Vector3(movementVector.x, 0, movementVector.z).magnitude * moveSpeed;
        playerAnimator.SetFloat("Speed", auxSpeed);
        playerAnimator.SetBool("Jump", isJumping);
    }

    void FixedUpdate()
    {
        if (!charController.isGrounded)
            movementVector.y += Physics.gravity.y * gravityMult * Time.deltaTime;
        else
        {
            movementVector.y = Physics.gravity.y * gravityMult * Time.deltaTime;
            isJumping = false;
        }
    }

    public void Bounce()
    {
        movementVector.y = bounceSpeed;
        charController.Move(Vector3.up * bounceSpeed * Time.deltaTime);
        // playerAnimator.SetBool("Jump", isJumping);
    }
    public GameObject[] GetModelParts()
    {
        return modelParts;
    }

}