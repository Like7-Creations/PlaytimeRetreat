using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManagerTwo : MonoBehaviour
{
   enum AbilityType
   {
        FreezeAbility,
        Shrink,
        Enlarge,
   }
    KeyCode[] key = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    AbilityType SelectedAbility = AbilityType.FreezeAbility;

    int AbilityIndex;
    void Update()
    {
        switch(AbilityIndex)
        {
              case 1:
                  SelectedAbility = AbilityType.FreezeAbility;
                  break;
              case 2:
                  SelectedAbility = AbilityType.Enlarge;
                  break;
              case 3:
                  SelectedAbility = AbilityType.Shrink;
                  break;
        }
        
        for (int i = 0; i < key.Length; i++)
        {
            if (Input.GetKeyDown(key[i]))
            {
                AbilityIndex = i + 1;
                Debug.Log(AbilityIndex);
            }
        }
    }
}
