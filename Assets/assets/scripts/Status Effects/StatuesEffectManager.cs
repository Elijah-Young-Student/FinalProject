using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class StatusEffectManager : MonoBehaviour
{
    public static StatusEffectManager Instance;

    [Header("State")]
    public bool statusSelectionMode;

    private Action<StatusEffect> onSelected;

    [Header("UI")]
    public TextMeshProUGUI descriptionText;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!statusSelectionMode)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            StatusToken token = hit.collider.GetComponentInParent<StatusToken>();

            if (token != null)
            {
                // HOVER → show description
                descriptionText.enabled = true;
                descriptionText.text = token.effect.description;

                // CLICK → select
                if (Input.GetMouseButtonDown(0))
                {
                    onSelected?.Invoke(token.effect);
                    EndSelection();
                }

                return;
            }
        }

        // nothing hit
        descriptionText.enabled = false;
    }

    public void StartSelection(Action<StatusEffect> callback)
    {
        statusSelectionMode = true;
        onSelected = callback;

        descriptionText.enabled = false;
    }

    public void EndSelection()
    {
        statusSelectionMode = false;
        onSelected = null;
        descriptionText.enabled = false;
    }

    public IEnumerator MoveToken(GameObject token, Vector3 targetPos, float speed)
    {
        while (token != null && Vector3.Distance(token.transform.position, targetPos) > 0.01f)
        {
            token.transform.position = Vector3.MoveTowards(
                token.transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            yield return null;
        }

        if (token != null)
            token.transform.position = targetPos;
    }
}