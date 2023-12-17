using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalDistanceToPlayer = 1.5f;
    public float horizontalDistanceToPlayer = 3;
    public float oclusionRaycastDistance = 5f;
    public float groundLerpSpeed = 5f;
    public float airborneLerpSpeed = 10f;
    public float rotationSpeed = 5f;
    public float slerpSpeed = 2f;
    public float oclusionMinHeight = 1.5f;
    private RaycastHit hit;
    private bool occlusionOccurring = false;
    private bool isRightMouseButtonDown = false;
    private Vector3 lastMousePosition;
    private Quaternion initialRotation;
    private CharacterController characterController;
    public GameObject lowestStagePoint;
    private bool isVerticalMovement;
    private float objectHeight;
    private float mouseX;
    private float mouseY;
    private float currentLerpSpeed;

    void Start()
    {
        player = PlayerController.uniqueInstance.transform;
        initialRotation = transform.rotation;
        characterController = player.GetComponent<CharacterController>();
    }

    void LateUpdate()
    {
        if (player.transform.position.y >= lowestStagePoint.transform.position.y - 1)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isRightMouseButtonDown = true;
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                isRightMouseButtonDown = false;
                StartCoroutine(ReturnToInitialRotation());
            }

            if (isRightMouseButtonDown)
            {
                mouseX = Input.mousePosition.x - lastMousePosition.x;
                mouseY = Input.mousePosition.y - lastMousePosition.y;

                transform.RotateAround(player.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
                transform.RotateAround(player.position, transform.right, -mouseY * 2 * rotationSpeed * Time.deltaTime);

                lastMousePosition = Input.mousePosition;
            }

            Vector3 defaultPosition = new Vector3(player.position.x, player.position.y + verticalDistanceToPlayer, player.position.z - horizontalDistanceToPlayer);

            isVerticalMovement = Mathf.Abs(characterController.velocity.y) > 0.1f;

            if (isVerticalMovement)
            {
                currentLerpSpeed = characterController.isGrounded ? groundLerpSpeed : airborneLerpSpeed;
                transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * currentLerpSpeed);
            }
            else if (Physics.Raycast(player.position, (defaultPosition - player.position).normalized, out hit, oclusionRaycastDistance))
            {
                objectHeight = hit.collider.bounds.size.y;

                if (objectHeight > player.position.y * oclusionMinHeight)
                {
                    Vector3 targetPosition = hit.point;
                    targetPosition.y = player.position.y + verticalDistanceToPlayer;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * groundLerpSpeed);
                    occlusionOccurring = true;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30f, 0f, 0f), Time.deltaTime * slerpSpeed);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * groundLerpSpeed);
                    transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * slerpSpeed);
                }
            }
            else
            {
                occlusionOccurring = false;
                transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * groundLerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * slerpSpeed);
            }
        }
        else
        {
            Vector3 cameraLowestPosition = new Vector3(transform.position.x, lowestStagePoint.transform.position.y + 1, transform.position.z);
            transform.position = cameraLowestPosition;
        }
    }

    IEnumerator ReturnToInitialRotation()
    {
        yield return new WaitForSeconds(0.1f);

        while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * slerpSpeed);
            yield return null;
        }
        occlusionOccurring = false;
    }
}
