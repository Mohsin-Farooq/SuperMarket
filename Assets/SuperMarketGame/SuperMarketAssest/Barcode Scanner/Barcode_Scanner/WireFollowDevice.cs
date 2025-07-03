using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireFollowDevice : MonoBehaviour
{
    public Transform device; // Only assign the moving device
    public Transform wireMesh; // Assign the wire mesh (pivot at static end, up = wire direction)

    private LineRenderer lineRenderer;
    private Vector3 staticEndWorldPos;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        // Cache world position of the fixed end
        staticEndWorldPos = transform.position;
    }

    void Update()
    {
        // Update LineRenderer points
        lineRenderer.SetPosition(0, staticEndWorldPos);
        lineRenderer.SetPosition(1, device.position);

        if (wireMesh != null)
        {
            Vector3 direction = device.position - staticEndWorldPos;
            float length = direction.magnitude;

            // Set mesh position at static end
            wireMesh.position = staticEndWorldPos;

            // Align mesh to face the device with Y axis as forward
            if (direction != Vector3.zero)
                wireMesh.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            //  Scale mesh along its Y axis
            wireMesh.localScale = new Vector3(
                wireMesh.localScale.x,
                length,
                wireMesh.localScale.z
            );
        }
    }
}
