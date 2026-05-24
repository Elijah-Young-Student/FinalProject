using UnityEngine;

public class BarbarianDice : MonoBehaviour, IDice
{
    // The 6 face normals in local space
    private static readonly Vector3[] faceNormals = new Vector3[]
    {
        Vector3.up,       // Top
        Vector3.down,     // Bottom
        Vector3.forward,  // Front
        Vector3.back,     // Back
        Vector3.right,    // Right
        Vector3.left      // Left
    };

    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Rigidbody GetRigidbody() => rb;

    public string GetFaceSide()
    {
        Vector3 worldUp = Vector3.up;
        float bestDot = float.MinValue;
        Vector3 bestFaceNormal = Vector3.zero;
        string bestFaceName = "";

        string[] faceNames = { "6✷", "1s", "2s", "5❤", "4❤", "3s" };

        for (int i = 0; i < faceNormals.Length; i++)
        {
            // Convert the local face normal to world space
            Vector3 worldNormal = transform.TransformDirection(faceNormals[i]);

            // Dot product: 1 = perfectly aligned with up, -1 = opposite
            float dot = Vector3.Dot(worldNormal, worldUp);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestFaceNormal = worldNormal;
                bestFaceName = faceNames[i];
            }
        }

        // Debug.Log($"Most vertical face: {bestFaceName} (dot: {bestDot:F3})");
        return bestFaceName;
    }
}