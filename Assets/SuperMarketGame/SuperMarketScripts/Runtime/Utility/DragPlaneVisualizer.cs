using UnityEngine;


[ExecuteAlways]
public class DragPlaneVisualizer : MonoBehaviour
{
    [Header("Plane Settings")]
    public float planeZ = 0.5f;                      // Your scanner's Z position
    public float planeSize = 5f;                     // Size of the grid
    public int gridLines = 10;                       // Number of grid lines
    public Color planeColor = Color.green;           // Grid color
    public Color normalColor = Color.red;            // Normal arrow color

    [Header("Optional: Ray Debugging")]
    public bool drawTestRay = false;
    public Vector3 testRayOrigin = new Vector3(0, 0, -5f);
    public Vector3 testRayDirection = Vector3.forward;
    public Color rayColor = Color.blue;

    private void OnDrawGizmos()
    {
        // Draw Grid
        Gizmos.color = planeColor;
        float halfSize = planeSize * 0.5f;

        for (int i = 0; i <= gridLines; i++)
        {
            float t = i / (float)gridLines;
            float x = Mathf.Lerp(-halfSize, halfSize, t);
            float y = Mathf.Lerp(-halfSize, halfSize, t);

            // Horizontal lines (Y)
            Gizmos.DrawLine(new Vector3(-halfSize, y, planeZ), new Vector3(halfSize, y, planeZ));

            // Vertical lines (X)
            Gizmos.DrawLine(new Vector3(x, -halfSize, planeZ), new Vector3(x, halfSize, planeZ));
        }

        // Draw Normal Arrow
        Gizmos.color = normalColor;
        Vector3 center = new Vector3(0, 0, planeZ);
        Vector3 normalEnd = center + Vector3.forward * 1f;
        Gizmos.DrawLine(center, normalEnd);
        Gizmos.DrawSphere(normalEnd, 0.05f);

        // Optional Test Ray (For seeing how rays would hit the plane)
        if (drawTestRay)
        {
            Gizmos.color = rayColor;
            Gizmos.DrawRay(testRayOrigin, testRayDirection * 10f);
        }
    }
}