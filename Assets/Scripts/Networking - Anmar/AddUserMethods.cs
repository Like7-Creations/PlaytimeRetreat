using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddUserMethods : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Http.HttpResponseEvent += AddUserResponse;
    }
    private void OnDestroy()
    {
        Http.HttpResponseEvent -= AddUserResponse;
    }

    void Start()
    {
        AddUser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUser()
    {
        User user = new User("Migatte", 99, /*"Nowhere",*/ "blabla@bla.com", "05055050", "nowherestill", "NoGokui", "hellotherebrugo");

        string json = JsonUtility.ToJson(user);
        
        //Http.HttpResponseEvent += AddUserResponse;
        StartCoroutine(Http.POST(ServerEndPoint.instance.AddUserEndPoint, json));
    }

    void AddUserResponse(string jsonResponse, bool successful)
    {
        print(jsonResponse);
        if (successful) print("added a user");
        else print("didnt work lol");
    }
}
