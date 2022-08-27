using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PickUpThrow : MonoBehaviour
{
    public PickUpNetComp pickupComp;

    PlayerController player;
    [SerializeField] public bool holding;
    [SerializeField] public bool hasplayer;
    [SerializeField] bool chargingg;
    public float baseforce;
    [SerializeField] float throwForce;
    public float timer;
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
        pick.performed += PickUpObj;

        charging = PickUpDrop.PickupDropThrow.Charge;
        charging.Enable();
        charging.performed += ChargeObj;

        Throwing = PickUpDrop.PickupDropThrow.Throw;
        Throwing.Enable();
        Throwing.performed += ThrowObj;
    }

    private void OnDisable()
    {
        pick.Disable();
        charging.Disable();
        Throwing.Disable();
    }

    void Start()
    {
        //player = FindObjectOfType<PlayerController>();
        pickupComp = GetComponent<PickUpNetComp>();
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame

    public void PickUpObj(InputAction.CallbackContext context)
    {
        Picker();
    }
    public void ChargeObj(InputAction.CallbackContext context)
    {
        Charging();
    }
    public void ThrowObj(InputAction.CallbackContext context)
    {
        Throw();
    }

    public void Picker()
    {
        if (hasplayer && !holding && rb.mass<20)
        {
            this.rb.isKinematic = true;
            rb.detectCollisions = false;
            transform.parent = Camera.main.transform;
            holding = true;
        }
    }
    public void Charging()
    {
        if (holding) { chargingg = true; throwForce += Time.deltaTime * 200; }
    }
    public void Throw()
    {
        if (holding && timer >= .5f)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            transform.parent = null;
            chargingg = false;
            rb.AddForce(Camera.main.transform.forward * throwForce);
            throwForce = baseforce;
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
        rb.isKinematic = holding;

        if(holding) timer += Time.deltaTime;

        if (chargingg) throwForce += Time.deltaTime * 200;

        //Debug.Log("The Holding Bool is " + holding);
    } 
}
