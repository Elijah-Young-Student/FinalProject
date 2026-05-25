using UnityEngine;

public interface IDice
{
    string GetFaceSide();
    Rigidbody GetRigidbody();
    Vector3 GetOrigin();
}