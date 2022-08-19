using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFreezeAbility : MonoBehaviour
{
    //Potentially Deprecated

    public AbilityTargeting targeting;

    public GameObject selectedObj;

    public Color frozenColor;
    public Color finalColor;

    void Start()
    {
        targeting = GetComponent<AbilityTargeting>();
    }

    // Update is called once per frame
    void Update()
    {
        selectedObj = targeting.targetObj;

        /*if (Input.GetKeyDown(KeyCode.K))
        {
            if (targeting.targeting)
            {
                if (!selectedObj.GetComponent<ObjEffect>().freezeActive)
                {
                    selectedObj.GetComponent<ObjEffect>().freezeActive = true;
                    selectedObj.GetComponent<ObjEffect>().UnfreezeObject(true);
                    FreezeObject(selectedObj);
                    print("Object is Frozen");
                }
            }
        }*/
    }

    void FreezeObject(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
    }
}
