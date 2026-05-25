using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLauncher : MonoBehaviour
{
    public float force = 10f;
    public float maxAngleOffset = 15f;

    public void Launch(GameObject Die)
    {
        Rigidbody rb = Die.GetComponent<Rigidbody>();

        if (rb != null && Camera.main != null)
        {
            // generate direction difference
            float randomPitch = Random.Range(-maxAngleOffset, maxAngleOffset);
            float randomYaw = Random.Range(-maxAngleOffset, maxAngleOffset);
            Quaternion deviation = Quaternion.Euler(randomPitch, randomYaw, 0f);

            // determine final direction
            Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward) * Quaternion.Euler(-45, 0, 0);
            Vector3 finalDirection = (cameraRotation * deviation) * Vector3.forward;

            rb.velocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            rb.transform.position = Camera.main.transform.position;
            rb.AddForce(finalDirection * force, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * (force * 0.5f), ForceMode.Impulse);
        }
    }

    public void ReturnDiceToOrign(GameObject Die)
    {
        // set the position to the origin created when the game starts
        Die.transform.position = Die.GetComponent<IDice>().GetOrigin();
    }
}
