using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationController : MonoBehaviour
{
    public Animator anima;
    public int Position = 1;

    public bool inspect;
    public Vector3 currentLoc;
    public Vector3 targetLoc;
    public Vector3 currentRot;
    public Vector3 targetRot;

    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Animator>();
        currentLoc = transform.position;
        targetLoc = transform.position;
        currentRot = transform.eulerAngles;
        targetRot = transform.eulerAngles;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject item = hit.collider.gameObject;
            
            if (Input.GetMouseButtonDown(0))
            {
                print(item.name);
                if (!inspect)
                {
                    inspect = true;
                    switch (item.name)
                    {
                        case "Smack":
                            break;
                        case "Strudy Blow":
                            break;
                        case "Fortitude":
                            break;
                        case "Overpower":
                            break;
                        case "Mighty Blow":
                            break;
                        case "Crit Bash":
                            break;
                        case "Reckless":
                            break;
                        case "Thick Skin":
                            break;
                        case "Rage!":
                            break;

                    }
                } else if (inspect)
                inspect = false;
            }
        }
        InputHandler();
    }

    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Position = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Position = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            Position = 3;
        anima.SetInteger("Position", Position);
    }
}
