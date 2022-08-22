using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
   public string name;
   public int age;
   public string address;
   public string email;
   public string phonenumber;
   public string nationality;
   public string username;
   public string password;

    public string Name   { get { return name; } }
    public int Age { get { return age; } }
    //public string Address { get { return address; } }
    public string Email { get { return email; } }
    public string Phonenumber { get { return phonenumber; } }
    public string Nationality { get { return nationality; } }
    public string Username { get { return username; } }
    public string Password { get { return password; } }


    public User(string name,
    int age,
    //string address,
    string email,
    string phonenumber,
    string nationality,
    string username,
    string password)
    {
        this.name = name;
        this.age = age;
        //this.address = address;
        this.email = email;
        this.phonenumber = phonenumber;
        this.nationality = nationality;
        this.username = username;
        this.password = password;
    }
}
