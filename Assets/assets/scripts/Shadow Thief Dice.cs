using UnityEngine;

public class ShadowThiefDice : MonoBehaviour, IDice
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

    public Vector3 Origin;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Origin = transform.position;
    }


    public Rigidbody GetRigidbody() => rb;

    public Vector3 GetOrigin() => Origin;

    public string GetFaceSide()
    {
        Vector3 worldUp = Vector3.up;
        float bestDot = float.MinValue;
        Vector3 bestFaceNormal = Vector3.zero;
        string bestFaceName = "";

        string[] faceNames = { "3b", "5c", "6s", "1d", "2d", "4b" };

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