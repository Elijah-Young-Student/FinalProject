using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FlipGameObject : MonoBehaviour
{
    public Animator anima;
    public bool flipped = false;

    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(1))
            {
                GameObject item = hit.collider.gameObject;
                print(item.name);
                if (item.transform == transform)
                {
                    flipped = !flipped;
                    if (flipped)
                        anima.SetBool("Flipped", true);
                    else if (!flipped)
                        anima.SetBool("Flipped", false);
                }
            }
        }
    }
}