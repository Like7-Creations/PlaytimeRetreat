using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjEffect : MonoBehaviour
{
    AbilityTargeting targeting;

    Rigidbody rbody;
    //Transform dirArrow;
    Collider objCollider;
    Renderer colorRenderer;

    //Frozen Momentum Stuff
    public Vector3 direction;
    public Vector3 hitPoint;

    //Object Scaling Values
    public Vector3 originalObjScale;

    /*float maxForce = 50;
    public float accumulatedForce;*/

    float originalMass;

    //Timer Bool
    public bool effectTimer = true;

    //Effect Bools
    public bool freezeActive;

    public bool shrinkActive;
    public bool growActive;

    public bool bounceActive;
    public bool frictionInactive;

    //public bool heavyObj;
    //public bool lightObj;

    Color originalObjColor;
    Color highlightedColor;

    /*Color frozenColor;
    Color finalColor;*/

    TestNetManager testmanager;


    void Awake()
    {
        this.gameObject.AddComponent<PickUpThrow>();
        this.gameObject.AddComponent<PickUpNetComp>();
        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.AddComponent<RBNetComp>();
        targeting = FindObjectOfType<AbilityTargeting>();

        rbody = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();
        colorRenderer = GetComponent<Renderer>();

        originalMass = rbody.mass;

        originalObjScale = gameObject.transform.localScale;

        originalObjColor = colorRenderer.material.color;
        highlightedColor = FindObjectOfType<ObjEffect>().highlightedColor;
        testmanager = FindObjectOfType<TestNetManager>();

        /*frozenColor = FindObjectOfType<ObjEffect>().frozenColor;
        finalColor = FindObjectOfType<ObjEffect>().finalColor;*/

        //dirArrow = transform.GetChild(0);
    }

    void Update()
    {
        if(testmanager.localPlayer != null) 
        {
            targeting = testmanager.localPlayer.GetComponent<AbilityTargeting>();
            if (targeting.targeting)
            {
                if (gameObject == targeting.targetObj)
                {
                    colorRenderer.material.color = highlightedColor;
                }

                else
                {
                    colorRenderer.material.color = originalObjColor;
                }
            }
        }



    }

    /*public void UnfreezeObject(bool state)
    {
        print("Froze Object");

        freezeActive = state;
        rbody.isKinematic = state;

        if (state)
        {
            //colorRenderer.material.SetColor("Emission Color", frozenColor);

            StartCoroutine(EffectCountdown());
            print($"{gameObject.name} has been frozen");
        }

        if (!state)
        {
            StopAllCoroutines();

            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            print($"{gameObject.name} is no longer frozen");

            freezeActive = false;

            /*transform.GetChild(0).gameObject.SetActive(state);

            if (accumulatedForce < 0)
            {
                return;
            }

            direction = transform.position - hitPoint;

            rbody.AddForceAtPosition(direction * accumulatedForce, hitPoint, ForceMode.Impulse);

            accumulatedForce = 0;
        }
    }*/

    /*public void AccumulateForce(float amount, Vector3 point)
    {
        if (!freezeActive)
            return;

        dirArrow.gameObject.SetActive(true);
        float scale = Mathf.Min(dirArrow.localScale.z + 0.3f, 1.8f);

        accumulatedForce = Mathf.Min(accumulatedForce += amount, maxForce);
        hitPoint = point;

        direction = transform.position - hitPoint;
        transform.GetChild(0).rotation = Quaternion.LookRotation(direction);
    }*/

    //----------Scale n Mass Functions----------
    public void ModifyObjScalenMass(Vector3 scaleVal, float mass)
    {
        transform.localScale = scaleVal;
        rbody.mass = mass;
    }
    
    public void ReturnToNormalSize(bool state)
    {
        if (state)
        {
            if (shrinkActive)
            {
                StartCoroutine(EffectCountdown());
                print($"{gameObject.name} shrunk & lost mass");
            }

            else if (growActive)
            {
                StartCoroutine(EffectCountdown());
                print($"{gameObject.name} grew & gained mass");
            }
        }

        if (!state)
        {
            StopAllCoroutines();

            transform.localScale = originalObjScale;
            rbody.mass = originalMass;

            colorRenderer.material.color = originalObjColor;

            if (shrinkActive)
                shrinkActive = false;
            if (growActive)
                growActive = false;
        }
    }
    //----------Scale n Mass Functions----------


    //----------Friction Functions----------
    public void EnableFriction(bool state)
    {
        if (state)
        {
            if (frictionInactive)
            {
                StartCoroutine(EffectCountdown());
                print($"{gameObject.name}'s friction is inactive");
            }
        }

        if (!state)
        {
            StopAllCoroutines();
            frictionInactive = false;
            objCollider.material.dynamicFriction = 0.6f;
            rbody.mass = originalMass;
            objCollider.material.frictionCombine = PhysicMaterialCombine.Average;

            print($"{gameObject.name}'s friction is Active");
        }
    }

    public void DisableFriction(Collider col)
    {
        col = GetComponent<Collider>();

        col.material.dynamicFriction = 0.2f;
        col.attachedRigidbody.mass += 20;

        col.material.frictionCombine = PhysicMaterialCombine.Minimum;
        print("Object is Slippery Now");
    }
    //----------Friction Functions----------


    //----------Bounciness Functions----------
    public void EnableBounce(Collider col)
    {
        col = GetComponent<Collider>();

        col.material.bounciness = 1;
        col.material.bounceCombine = PhysicMaterialCombine.Maximum;
        print("Bouncyness Activate");
    }

    public void DisableBounce(bool state)
    {
        if (state)
        {
            if (bounceActive)
            {
                StartCoroutine(EffectCountdown());
                print($"{gameObject.name} is Bouncy now");
            }

        }

        if (!state)
        {
            StopAllCoroutines();

            bounceActive = false;
            objCollider.material.bounciness = 0;
            objCollider.material.bounceCombine = PhysicMaterialCombine.Average;
            print($"{gameObject.name} is no longer bouncy");
        }
    }
    //----------Bounciness Functions----------

    /*public void ResetObjMass(bool state)
    {
        if (state)
        {
            if (heavyObj)
            {
                StartCoroutine(EffectCountdown());
                print($"{gameObject.name}'s mass has increased.");
            }

            else if (lightObj)
            {
                StartCoroutine(EffectCountdown());
                print($"{gameObject.name}'s mass has decreased.");
            }
        }

        if (!state)
        {
            StopAllCoroutines();

            rbody.mass = originalMass;
            colorRenderer.material.color = originalObjColor;

            if (heavyObj)
                heavyObj = false;
            if (lightObj)
                lightObj = false;
        }
    }*/

    public IEnumerator EffectCountdown()
    {
        if (effectTimer)
        {
            for (int i = 0; i < 20; i++)
            {
                float delay = 1;

                if (i > 5)
                    delay = 0.5f;

                if (i > 15)
                    delay = 0.25f;

                yield return new WaitForSeconds(delay);
            }

            /*//For Resetting Frozen Object
            if (freezeActive)
                UnfreezeObject(false);
            //For Resetting Frozen Object*/

            //For Resetting Object Scale
            if (shrinkActive)
                ReturnToNormalSize(false);
            if (growActive)
                ReturnToNormalSize(false);
            //For Resetting Object Scale

            /*//For Resetting Object Mass
            if (heavyObj)
                ResetObjMass(false);
            if (lightObj)
                ResetObjMass(false);
            //For Resetting Object Mass*/

            //For Resetting Object Bounciness
            if (bounceActive)
                DisableBounce(false);
            //For Resetting Object Bounciness

            //For Resetting Object Friction
            if (frictionInactive)
                EnableFriction(false);
            //For Resetting Object Friction
        }

        if (!effectTimer)
        {
            StopAllCoroutines();
        }
    }
}