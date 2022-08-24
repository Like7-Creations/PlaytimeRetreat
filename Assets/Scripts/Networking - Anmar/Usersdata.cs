using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usersdata : MonoBehaviour
{
    User[] users;

    public User[] Users
    {
        get { return users; }
    }

    public Usersdata(User[] users)
    {
        this.users = users;
    }
}
