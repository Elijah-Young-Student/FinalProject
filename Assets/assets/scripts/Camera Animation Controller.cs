using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationController : MonoBehaviour
{
    public Animator anima;
    public int Position = 1;

    public float speed = 15;

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
                // print(item.name);
                if (!inspect)
                {
                    switch (item.name)
                    {
                        case "Smack":
                            targetLoc = new Vector3(-1.02499998f, 5.46000004f, -1.40699995f);
                            inspect = true;
                            anima.enabled = false;
                            targetRot = new Vector3(90, 0, 0);
                            break;
                        case "Sturdy Blow":
                            targetLoc = new Vector3(-0.680000007f, 5.46000004f, -1.40699995f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Fortitude":
                            targetLoc = new Vector3(-1.02999997f, 5.46000004f, -1.85399997f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Overpower":
                            targetLoc = new Vector3(-0.694000006f, 5.46000004f, -1.85399997f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Mighty Blow":
                            targetLoc = new Vector3(0.356999993f, 5.46000004f, -1.41700006f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Crit Bash":
                            targetLoc = new Vector3(0.699000001f, 5.46000004f, -1.41700006f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Reckless":
                            targetLoc = new Vector3(0.349999994f, 5.46000004f, -1.85399997f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Thick Skin":
                            targetLoc = new Vector3(0.67900002f, 5.46000004f, -1.85399997f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Rage!":
                            targetLoc = new Vector3(-0.172999993f, 5.46000004f, -1.94200003f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Barbarian CP Dial":
                            targetLoc = new Vector3(0.579999983f, 5.78999996f, -0.810000002f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Barbarian Health":
                            targetLoc = new Vector3(-0.810000002f, 5.78999996f, -0.769999981f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Barbarian Character Sheet":
                            targetLoc = new Vector3(-2.01999998f, 6.54699993f, -1.25600004f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Barbarian Character Board":
                            targetLoc = new Vector3(-0.133000001f, 6.03999996f, -1.5572741f);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                        case "Barbarian Turn Card":
                            targetLoc = new Vector3(0, 0, 0);
                            targetRot = new Vector3(90, 0, 0);
                            inspect = true;
                            anima.enabled = false;
                            break;
                    }
                }
                else if (inspect)
                {
                    anima.enabled = true;
                    inspect = false;
                }
            }
        }
        InputHandler();

        currentLoc = transform.position;
        currentRot = transform.eulerAngles;
        transform.position = Vector3.MoveTowards(transform.position, targetLoc, speed / 2 * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(currentRot), Quaternion.Euler(targetRot), speed / 2 * speed * Time.deltaTime);
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
