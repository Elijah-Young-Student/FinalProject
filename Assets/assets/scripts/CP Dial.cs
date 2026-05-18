using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPDial : MonoBehaviour
{
    private Quaternion currentRotation;
    private Quaternion targetRotation;
    public float speed = 5;

    public enum CPValues { Zero = 0, One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Eleven = 11, Twelve = 12, Thirteen = 13, Fourteen = 14, Fifteen = 15 }

    private void Update()
    {
        if (currentRotation != targetRotation)
        {
            float step = speed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        }
    }

    public void SetTargetRotation(Quaternion targetRotation)
    {
        this.targetRotation = targetRotation;
    }

    public void SetTargetRotation(CPValues value)
    {
        switch (value)
        {
            case CPValues.Zero:
                targetRotation = Quaternion.Euler(-90, -90, -90);
                break;
            case CPValues.One:
                targetRotation = Quaternion.Euler(-112.05f, -90, -90);
                break;
            case CPValues.Two:
                targetRotation = Quaternion.Euler(-134.1f, -90, -90);
                break;
            case CPValues.Three:
                targetRotation = Quaternion.Euler(-157.6f, -90, -90);
                break;
            case CPValues.Four:
                targetRotation = Quaternion.Euler(-179, -90, -90);
                break;
            case CPValues.Five:
                targetRotation = Quaternion.Euler(-203, -90, -90);
                break;
            case CPValues.Six:
                targetRotation = Quaternion.Euler(-225.05f, -90, -90);
                break;
            case CPValues.Seven:
                targetRotation = Quaternion.Euler(-247.95f, -90, -90);
                break;
            case CPValues.Eight:
                targetRotation = Quaternion.Euler(-269.35f, -90, -90);
                break;
            case CPValues.Nine:
                targetRotation = Quaternion.Euler(-292.05f, -90, -90);
                break;
            case CPValues.Ten:
                targetRotation = Quaternion.Euler(-313.08f, -90, -90);
                break;
            case CPValues.Eleven:
                targetRotation = Quaternion.Euler(-336.4f, -90, -90);
                break;
            case CPValues.Twelve:
                targetRotation = Quaternion.Euler(-375.3f, -90, -90);
                break;
            case CPValues.Thirteen:
                targetRotation = Quaternion.Euler(-381.026f, -90, -90);
                break;
            case CPValues.Fourteen:
                targetRotation = Quaternion.Euler(-403.2f, -90, -90);
                break;
            case CPValues.Fifteen:
                targetRotation = Quaternion.Euler(-425.617f, -90, -90);
                break;
        }
    }
}
