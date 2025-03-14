using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 10f;

    [Header("Zoom Settings")]
    [SerializeField] float zoomSpeed = 10f;
    [SerializeField] float minZoom = 5f;
    float maxZoom;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        maxZoom = startPosition.y;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        CheckForResetPosition();
    }

    void HandleMovement()
    {
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Vector3 move = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 direction = transform.forward * scroll * zoomSpeed;
            Vector3 newPosition = transform.position + direction;

            if (newPosition.y >= minZoom && newPosition.y <= maxZoom)
            {
                transform.position = newPosition;
            }
        }
    }

    void CheckForResetPosition()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = startPosition;
        }
    }
}
