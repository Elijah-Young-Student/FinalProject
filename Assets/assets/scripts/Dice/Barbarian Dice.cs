using System;
using System.Collections;
using Unity.Mathematics;
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

    public Vector3 Origin;

    private string forcedFace = "";

    private string[] faceNames = { "6✷", "1s", "2s", "5❤", "4❤", "3s" };

    public void ForceFace(string face)
    {
        forcedFace = face;
    }

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
        // force face of die
        if (!string.IsNullOrEmpty(forcedFace))
        {
            string result = forcedFace;
            forcedFace = "";
            return result;
        }

        // find the new face after roll
        Vector3 worldUp = Vector3.up;
        float bestDot = float.MinValue;
        Vector3 bestFaceNormal = Vector3.zero;
        string bestFaceName = "";



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

    public IEnumerator AlignToFace(string face, float speed = 100f)
    {
        Vector3 localNormal = faceNormals[Array.IndexOf(faceNames, face)];
        Vector3 worldNormal = transform.TransformDirection(localNormal);

        Quaternion targetRot =
            Quaternion.FromToRotation(transform.up, worldNormal) * transform.rotation;

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                speed * Time.deltaTime
            );

            yield return null;
        }

        transform.rotation = targetRot;
    }
}