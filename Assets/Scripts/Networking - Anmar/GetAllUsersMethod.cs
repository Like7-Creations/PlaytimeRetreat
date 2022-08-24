using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllUsersMethod : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Http.HttpResponseEvent += GetAllUsersResponse;
    }
    private void OnDestroy()
    {
        Http.HttpResponseEvent -= GetAllUsersResponse;
    }

    void Start()
    {
        GetAllUsers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllUsers()
    {
        StartCoroutine(Http.GET(ServerEndPoint.instance.GetAllUserEndPoint,""));
    }

    void GetAllUsersResponse(string jsonResponse, bool successful)
    {
        if (successful) print("got all users lmao");
            
        Usersdata userdata = JsonUtility.FromJson<Usersdata>(jsonResponse);

        for (int i = 0; i < userdata.Users.Length; i++)
        {
            print(userdata.Users[i].Name);
        }

        //if(print(js))
        print(jsonResponse);
    }
}
