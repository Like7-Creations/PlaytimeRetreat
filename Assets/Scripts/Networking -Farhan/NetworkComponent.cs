using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkComponent : MonoBehaviour
{
    [Header("Object Components")]
    //Object n Environment Specific Components
    public Rigidbody gRbody;

    public Collider objCollider;
    public Collider triggerCollider;

    public ObjEffect objEffect;
    //Object n Environment Specific Components
    [Space(10)]

    [Header("Contraption Components")]
    //Functional Specific Components
    public Prerequesets prerequisites;

    public ConveyorBelt conveyor;
    public DoorTrigger door;
    public Elevator elevator;
    public LaunchPad launchPad;
    //Functional Specific Components
    [Space(10)]
    
    [Header("Trigger Component")]
    //Trigger Specific Components
    public TriggerSystem trigger;
    //Trigger Specific Components
    [Space(10)]

    [Header("Player Components")]
    //Player specific Components
    public Transform playerTransform;
    public CapsuleCollider capCollider;
    
    public PlayerController pController;
    public MobileController mController;
    public PlayerCollision collision;
    
    public AbilityHolder ability;
    public AbilityTargeting targeting;
    
    public ObjFreezeAbility freezeAbility;
    public ObjMassAbility massAbility;
    public ObjScaleAbility scaleAbility;
    public ObjSurfaceAbility surfaceAbility;
    //Player specific Components


    //public Component[] objComponents = new Component[5];

    void Start()
    {
        //objComponents = gameObject.GetComponents(typeof(Component));

        if(gameObject.tag == "Player")
        {
            playerTransform = GetComponent<Transform>();
            capCollider = GetComponent<CapsuleCollider>();

            pController = GetComponent<PlayerController>();
            mController = GetComponent<MobileController>();
            collision = GetComponent<PlayerCollision>();

            ability = GetComponent<AbilityHolder>();
            targeting = GetComponent<AbilityTargeting>();

            freezeAbility = GetComponent<ObjFreezeAbility>();
            massAbility = GetComponent<ObjMassAbility>();
            scaleAbility = GetComponent<ObjScaleAbility>();
            surfaceAbility = GetComponent<ObjSurfaceAbility>();
        }

        else if (gameObject.tag== "Trigger")
        {
            objCollider = GetComponent<Collider>();
            trigger = GetComponent<TriggerSystem>();
        }

        else if (gameObject.tag == "Contraption")
        {
            if(gameObject.GetComponent<Elevator>() == true)
            {
                elevator = gameObject.GetComponent<Elevator>();
                prerequisites = gameObject.GetComponent<Prerequesets>();
            }

            else if (gameObject.GetComponent<DoorTrigger>() == true)
            {
                door = gameObject.GetComponent<DoorTrigger>();
                prerequisites = gameObject.GetComponent<Prerequesets>();
            }

            else if (gameObject.GetComponent<ConveyorBelt>() == true)
            {
                conveyor = gameObject.GetComponent<ConveyorBelt>();
                prerequisites = gameObject.GetComponent<Prerequesets>();
            }

            else if(gameObject.GetComponent<LaunchPad>() == true)
            {
                launchPad = gameObject.GetComponent<LaunchPad>();
                prerequisites = gameObject.GetComponent<Prerequesets>();
            }

        }

        //Gonna have to keep track of each component added to a game object. Maybe a parent network component,
        //and a couple of child network components that are specific to different types of objects.
        //The types of networking components we'd need is a
        //PlayerComponent, ObjectComponent, ConveyorComponent, DoorComponent, ElevatorComponent, TeleporterComponent, TriggerComponent,  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
