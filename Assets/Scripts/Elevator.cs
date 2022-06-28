using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    Vector3 velocity;
    [SerializeField] float Max_Velocity;
    public Transform elevator;
    public Transform[] waypoints;
    [SerializeField]float maxForce;
    [SerializeField] float mass;
    [SerializeField] float max_Speed;
    [SerializeField] float slowingradius;
    [SerializeField] bool UpDown;
    [SerializeField] bool hasplayer;

    [SerializeField]float timer;
    
    void Start()
    {
        velocity = Vector3.zero;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
        {
            hasplayer = hit.collider.gameObject == this.gameObject;
        }

        if (hasplayer)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (UpDown) UpDown = false;
                else UpDown = true;          
            }          
        }

        timer += Time.deltaTime;
        if (timer > 5) UpDown = true;
        if (timer >= 11) { UpDown = false; timer = 0; }

        if (UpDown)
        {
            Arrival(waypoints[0]);
        }

        else Arrival(waypoints[1]);
    }

    void Arrival(Transform obj)
    {
        var desired_velocity = obj.transform.position - elevator.transform.position;
        var dist = Vector3.Distance(elevator.position, obj.transform.position);

        if (dist < slowingradius)
        {
            desired_velocity = desired_velocity.normalized * Max_Velocity * (dist / slowingradius);
        }
        else
        {
            desired_velocity = desired_velocity.normalized * Max_Velocity;
        }

        var steering = desired_velocity - velocity;
        velocity = Vector3.ClampMagnitude(velocity + steering, max_Speed);
        elevator.position += velocity * Time.deltaTime;
    }
}
