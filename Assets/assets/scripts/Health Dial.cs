using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDial : MonoBehaviour
{
    public GameObject healthDialR;
    public GameObject healthDialL;

    public Vector3 currentHDRRot;
    public Vector3 targetHDRRot;
    public Vector3 currentHDLRot;
    public Vector3 targetHDLRot;

    public int health = 90;
    public float speed = 25;

    // Start is called before the first frame update
    void Start()
    {
        currentHDLRot = healthDialL.transform.eulerAngles;
        targetHDLRot = healthDialL.transform.eulerAngles;
        currentHDRRot = healthDialR.transform.eulerAngles;
        targetHDRRot = healthDialR.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        currentHDLRot = healthDialL.transform.eulerAngles;
        healthDialL.transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(currentHDLRot), Quaternion.Euler(targetHDLRot), step);
        currentHDRRot = healthDialR.transform.eulerAngles;
        healthDialR.transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(currentHDRRot), Quaternion.Euler(targetHDRRot), step);
    }

    public void SetHealth(int health)
    {
        this.health = health;

        //left dial | 10s place
        switch (Mathf.RoundToInt(health / 10 % 10))
        {
            case 0:
                targetHDLRot = new Vector3();
                break;
            case 1:
                targetHDLRot = new Vector3();
                break;
            case 2:
                targetHDLRot = new Vector3();
                break;
            case 3:
                targetHDLRot = new Vector3();
                break;
            case 4:
                targetHDLRot = new Vector3();
                break;
            case 5:
                targetHDLRot = new Vector3();
                break;
            case 6:
                targetHDLRot = new Vector3();
                break;
            case 7:
                targetHDLRot = new Vector3();
                break;
            case 8:
                targetHDLRot = new Vector3();
                break;
            case 9:
                targetHDLRot = new Vector3(270, 180, 0);
                break;

        }

        // right dial | 1s place
        switch (Mathf.RoundToInt(health % 10))
        {
            case 0:
                targetHDRRot = new Vector3(270, 180, 0);
                break;
            case 1:
                targetHDRRot = new Vector3();
                break;
            case 2:
                targetHDRRot = new Vector3();
                break;
            case 3:
                targetHDRRot = new Vector3();
                break;
            case 4:
                targetHDRRot = new Vector3();
                break;
            case 5:
                targetHDRRot = new Vector3();
                break;
            case 6:
                targetHDRRot = new Vector3();
                break;
            case 7:
                targetHDRRot = new Vector3();
                break;
            case 8:
                targetHDRRot = new Vector3();
                break;
            case 9:
                targetHDRRot = new Vector3();
                break;
        }
    }
}
