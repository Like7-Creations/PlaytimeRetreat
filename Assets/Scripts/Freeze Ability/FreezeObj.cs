using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeObj : MonoBehaviour
{
    Rigidbody rbody;

    Transform dirArrow;

    public Vector3 direction;
    public Vector3 hitPoint;

    float maxForce = 50;
    public float accumulatedForce;

    public bool freezeActive;

    Renderer colorRenderer;
    Color frozenColor;
    Color finalColor;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        frozenColor = FindObjectOfType<FreezeObj>().frozenColor;
        finalColor = FindObjectOfType<FreezeObj>().finalColor;

        dirArrow = transform.GetChild(0);
        colorRenderer = GetComponent<Renderer>();
    }

    public void FreezeObject(bool state)
    {
        print("Froze Object");

        freezeActive = state;
        rbody.isKinematic = state;

        if (state)
        {
            colorRenderer.material.SetColor("Emission Color", frozenColor);

            StartCoroutine(FreezeCountdown());
        }

        if (!state)
        {
            StopAllCoroutines();

            transform.GetChild(0).gameObject.SetActive(state);

            if (accumulatedForce < 0)
            {
                return;
            }

            direction = transform.position - hitPoint;

            rbody.AddForceAtPosition(direction * accumulatedForce, hitPoint, ForceMode.Impulse);

            accumulatedForce = 0;
        }
    }

    public void AccumulateForce(float amount, Vector3 point)
    {
        if (!freezeActive)
            return;

        dirArrow.gameObject.SetActive(true);
        float scale = Mathf.Min(dirArrow.localScale.z + 0.3f, 1.8f);

        accumulatedForce = Mathf.Min(accumulatedForce += amount, maxForce);
        hitPoint = point;

        direction = transform.position - hitPoint;
        transform.GetChild(0).rotation = Quaternion.LookRotation(direction);
    }

    public IEnumerator FreezeCountdown()
    {
        for (int i = 0; i < 20; i++)
        {
            float delay = 1;

            if (i > 4)
                delay = 0.5f;

            if (i > 14)
                delay = 0.25f;

            yield return new WaitForSeconds(delay);
        }

        FreezeObject(false);
    }
}