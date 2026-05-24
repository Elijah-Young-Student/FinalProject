using UnityEngine;

public interface IDice
{
    string GetFaceSide(); // Changed to return a string so your manager can collect it
    Rigidbody GetRigidbody();
}