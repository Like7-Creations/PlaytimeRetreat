using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterID : ScriptableObject
{
    public int ID;

    public void ChangeID(int changetoThis)
    {
        ID = changetoThis;
    }
}
