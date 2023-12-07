using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalDistanceToPlayer = 1.5f;
    public float horizontalDistanceToPlayer = 3;
    public float oclusionRaycastDistance = 5f;
    public float lerpSpeed = 5f;
    private RaycastHit hit;
    private bool occlusionOccurring = false;

    void Start()
    {
        player = PlayerController.uniqueInstance.transform;
    }

    void LateUpdate()
    {
        Vector3 defaultPosition = new Vector3(player.position.x, player.position.y + verticalDistanceToPlayer, player.position.z - horizontalDistanceToPlayer);

        if (Physics.Raycast(player.position, (defaultPosition - player.position).normalized, out hit, oclusionRaycastDistance))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = player.position.y + verticalDistanceToPlayer;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
            occlusionOccurring = true;
        }
        else if (occlusionOccurring)
        {
            Vector3 targetPosition = defaultPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                occlusionOccurring = false;
        }
        else
            transform.position = defaultPosition;
    }
}
