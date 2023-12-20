using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Quaternion initialRotation;
    public float verticalDistanceToPlayer = 1.5f;
    public float horizontalDistanceToPlayer = 3;
    public float oclusionRaycastDistance = 5f;
    public float groundLerpSpeed = 5f;
    public float airborneLerpSpeed = 10f;
    public float rotationSpeed = 5f;
    public float slerpSpeed = 2f;
    public float oclusionMinHeight = 1.5f;
    private float objectHeight;
    private float mouseX;
    private float mouseY;
    private float currentLerpSpeed;
    private RaycastHit hit;
    private bool occlusionOccurring = false;
    private bool isRightMouseButtonDown = false;
    private bool isVerticalMovement;
    private Vector3 lastMousePosition;
    private Vector3 defaultPosition;
    public GameObject lowestStagePoint;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (MainManager.Instance.playerController.transform.position.y >= lowestStagePoint.transform.position.y - 1)
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

                transform.RotateAround(MainManager.Instance.playerController.transform.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
                transform.RotateAround(MainManager.Instance.playerController.transform.position, transform.right, -mouseY * 2 * rotationSpeed * Time.deltaTime);

                lastMousePosition = Input.mousePosition;
            }

            defaultPosition = new Vector3(MainManager.Instance.playerController.transform.position.x, MainManager.Instance.playerController.transform.position.y + verticalDistanceToPlayer, MainManager.Instance.playerController.transform.position.z - horizontalDistanceToPlayer);

            isVerticalMovement = Mathf.Abs(MainManager.Instance.playerController.characterController.velocity.y) > 0.1f;

            if (isVerticalMovement)
            {
                currentLerpSpeed = MainManager.Instance.playerController.characterController.isGrounded ? groundLerpSpeed : airborneLerpSpeed;
                transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * currentLerpSpeed);
            }
            else if (Physics.Raycast(MainManager.Instance.playerController.transform.position, (defaultPosition - MainManager.Instance.playerController.transform.position).normalized, out hit, oclusionRaycastDistance))
            {
                objectHeight = hit.collider.bounds.size.y;

                if (objectHeight > MainManager.Instance.playerController.transform.position.y * oclusionMinHeight)
                {
                    Vector3 targetPosition = hit.point;
                    targetPosition.y = MainManager.Instance.playerController.transform.position.y + verticalDistanceToPlayer;
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
