using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPDial : MonoBehaviour
{
    public Vector3 currentRot;
    public Vector3 targetRot;
    public float speed = 25;

    public enum CPValues { Zero = 0, One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Eleven = 11, Twelve = 12, Thirteen = 13, Fourteen = 14, Fifteen = 15 }

    private void Start()
    {
        currentRot = transform.eulerAngles;
        targetRot = transform.eulerAngles;
    }

    private void Update()
    {
        currentRot = transform.eulerAngles;
        transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(currentRot), Quaternion.Euler(targetRot), speed * Time.deltaTime);
    }

    public void SetTargetRotation(CPValues value)
    {
        switch (value)
        {
            case CPValues.Zero:
                targetRot = new Vector3(0, 180, 0);
                break;
            case CPValues.One:
                targetRot = new Vector3(0, 202.3f, 0);
                break;
            case CPValues.Two:
                targetRot = new Vector3(0, 224.8f, 0);
                break;
            case CPValues.Three:
                targetRot = new Vector3(0, 247.3f, 0);
                break;
            case CPValues.Four:
                targetRot = new Vector3(0, 269.6f, 0);
                break;
            case CPValues.Five:
                targetRot = new Vector3(0, 292.8f, 0);
                break;
            case CPValues.Six:
                targetRot = new Vector3(0, 314.8f, 0);
                break;
            case CPValues.Seven:
                targetRot = new Vector3(0, 337.3f, 0);
                break;
            case CPValues.Eight:
                targetRot = new Vector3(0, 0, 0);
                break;
            case CPValues.Nine:
                targetRot = new Vector3(0, 22.5f, 0);
                break;
            case CPValues.Ten:
                targetRot = new Vector3(0, 43.2f, 0);
                break;
            case CPValues.Eleven:
                targetRot = new Vector3(0, 65.8f, 0);
                break;
            case CPValues.Twelve:
                targetRot = new Vector3(0, 87.3f, 0);
                break;
            case CPValues.Thirteen:
                targetRot = new Vector3(0, 110.3f, 0);
                break;
            case CPValues.Fourteen:
                targetRot = new Vector3(0, 132.8f, 0);
                break;
            case CPValues.Fifteen:
                targetRot = new Vector3(0, 155.8f, 0);
                break;
        }
    }
}
