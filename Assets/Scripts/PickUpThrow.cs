using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PickUpThrow : MonoBehaviour
{
    PlayerController player;
    [SerializeField] public bool holding;
    [SerializeField] public bool hasplayer;
    [SerializeField] bool chargingg;
    [SerializeField] float throwForce;
    float timer;
    bool bol;
    
    Rigidbody rb;

    public MechanicsControl PickUpDrop;
    private InputAction pick;
    private InputAction charging;
    private InputAction Throwing;

    private void Awake()
    {
        PickUpDrop = new MechanicsControl();
    }

    private void OnEnable()
    {
        pick = PickUpDrop.PickupDropThrow.PickUp;
        pick.Enable();
        pick.performed += Picker;

        charging = PickUpDrop.PickupDropThrow.Charge;
        charging.Enable();
        charging.performed += Charging;

        Throwing = PickUpDrop.PickupDropThrow.Throw;
        Throwing.Enable();
        Throwing.performed += Throw;
    }

    private void OnDisable()
    {
        pick.Disable();
        charging.Disable();
        Throwing.Disable();
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>(); 
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame

    public void Picker(InputAction.CallbackContext context)
    {
        if (hasplayer && !holding)
        {
            this.rb.isKinematic = true;
            rb.detectCollisions = false;
            transform.parent = Camera.main.transform;
            holding = true;
        }
    }
    public void Charging(InputAction.CallbackContext context)
    {
        if (holding) { chargingg = true; throwForce += Time.deltaTime * 200; }
    }
    public void Throw(InputAction.CallbackContext context)
    {
        if (holding && timer >= .5f)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            transform.parent = null;
            chargingg = false;
            rb.AddForce(Camera.main.transform.forward * throwForce);
            throwForce = 1000;
            timer = 0;
            holding = false;
            Debug.Log("throwing if holding");
        }
    }
    void Update()
    {
        //dist = Vector3.Distance(transform.position, player.transform.position);
        /*RaycastHit hit;
        if( Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
        {
            hasplayer = hit.collider.gameObject == this.gameObject;
        }
        else hasplayer = false;*/

        if(holding) timer += Time.deltaTime;

        if (chargingg) throwForce += Time.deltaTime * 200;

        Debug.Log("The Holding Bool is " + holding);
    } 
}
