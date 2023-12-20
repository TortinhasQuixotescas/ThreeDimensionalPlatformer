using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Move and Rotation
    private Vector3 movementVector;
    [SerializeField] private int moveSpeed = 8;
    [SerializeField] private int jumpSpeed = 20;
    [SerializeField] private int rotateSpeed = 10;
    [SerializeField] private int gravityMult = 5;
    public float bounceSpeed;
    private float ySpeed;
    private float auxSpeed;
    private bool isJumping;
    private float idleTimer;
    private int idleThreshold = 10;
    private bool lastGrounded;

    // Animation
    public Animator playerAnimator;
    [HideInInspector] public CharacterController characterController;
    private CameraController camController;
    public GameObject jumpParticle;
    public GameObject landingParticle;
    public GameObject[] modelParts;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        isJumping = false;
        idleTimer = 0f;
    }

    private void Start()
    {
        camController = FindObjectOfType<CameraController>();
        lastGrounded = true;
    }

    private void Update()
    {
        this.Blink();

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

        if (characterController.isGrounded)
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
        lastGrounded = characterController.isGrounded;
        characterController.Move(Time.deltaTime * new Vector3(movementVector.x * moveSpeed, movementVector.y, movementVector.z * moveSpeed));
        auxSpeed = new Vector3(movementVector.x, 0, movementVector.z).magnitude * moveSpeed;
        playerAnimator.SetFloat("Speed", auxSpeed);
        playerAnimator.SetBool("Jump", isJumping);
    }

    private void FixedUpdate()
    {
        if (!characterController.isGrounded)
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
        characterController.Move(Vector3.up * bounceSpeed * Time.deltaTime);
        // playerAnimator.SetBool("Jump", isJumping);
    }

    public void Blink()
    {
        if (MainManager.Instance.playerData.invulnerabilityCounter > 0)
        {
            MainManager.Instance.playerData.invulnerabilityCounter -= Time.deltaTime;
            MainManager.Instance.playerData.flashCounter -= Time.deltaTime;
            if (MainManager.Instance.playerData.flashCounter <= 0)
            {
                MainManager.Instance.playerData.flashCounter = MainManager.Instance.playerData.blinkDuration;
                foreach (GameObject part in modelParts)
                    part.SetActive(!part.activeSelf);
            }
            if (MainManager.Instance.playerData.invulnerabilityCounter <= 0)
            {
                foreach (GameObject part in modelParts)
                    part.SetActive(true);
            }
        }
    }

}