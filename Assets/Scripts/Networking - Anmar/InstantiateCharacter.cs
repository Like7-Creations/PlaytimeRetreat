using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCharacter : MonoBehaviour
{

    public CharacterID characterID;

    void Start()
    {
        InstantiateFunction(characterID.ID); 
    }
    void Update()
    {
    }

    void InstantiateFunction(int characterID)
    {
        GameObject playerlol = Resources.Load<GameObject>("Prefabs/PlayerController" + characterID);
        Instantiate(playerlol, Vector3.zero, Quaternion.identity);
    }
}
