using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPDial : MonoBehaviour
{
    public Vector3 currentRot;
    public Vector3 targetRot;
    public float speed = 25;

    public int CP;
    private void Start()
    {
        currentRot = transform.eulerAngles;
        targetRot = transform.eulerAngles;
    }

    private void Update()
    {
        // SetCP(CP);
        currentRot = transform.eulerAngles;
        transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(currentRot), Quaternion.Euler(targetRot), speed * Time.deltaTime);
    }

    public void SetCP(int CP)
    {
        this.CP = CP;
        switch (CP)
        {
            case 0:
                targetRot = new Vector3(0, 180, 0);
                break;
            case 1:
                targetRot = new Vector3(0, 202.3f, 0);
                break;
            case 2:
                targetRot = new Vector3(0, 224.8f, 0);
                break;
            case 3:
                targetRot = new Vector3(0, 247.3f, 0);
                break;
            case 4:
                targetRot = new Vector3(0, 269.6f, 0);
                break;
            case 5:
                targetRot = new Vector3(0, 292.8f, 0);
                break;
            case 6:
                targetRot = new Vector3(0, 314.8f, 0);
                break;
            case 7:
                targetRot = new Vector3(0, 337.3f, 0);
                break;
            case 8:
                targetRot = new Vector3(0, 0, 0);
                break;
            case 9:
                targetRot = new Vector3(0, 22.5f, 0);
                break;
            case 10:
                targetRot = new Vector3(0, 43.2f, 0);
                break;
            case 11:
                targetRot = new Vector3(0, 65.8f, 0);
                break;
            case 12:
                targetRot = new Vector3(0, 87.3f, 0);
                break;
            case 13:
                targetRot = new Vector3(0, 110.3f, 0);
                break;
            case 14:
                targetRot = new Vector3(0, 132.8f, 0);
                break;
            case 15:
                targetRot = new Vector3(0, 155.8f, 0);
                break;
        }
    }
}
