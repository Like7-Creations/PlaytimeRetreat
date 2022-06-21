using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpThrow : MonoBehaviour
{
    PlayerController player;
    [SerializeField] float dist;
    [SerializeField] bool holding;
    [SerializeField] bool hasplayer;
    [SerializeField] bool chargingg;
    [SerializeField] float throwForce;
    
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
       // Throwing.performed += Throw;
    }

    private void OnDisable()
    {
        pick.Disable();
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>(); 
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame

    void Picker(InputAction.CallbackContext context)
    {
        if (hasplayer)
        {
            this.rb.isKinematic = true;
            transform.parent = Camera.main.transform;
            holding = true;
        }
    }
    void Charging(InputAction.CallbackContext context)
    {
        if (holding)chargingg = true;
    }
    void Throw(/*InputAction.CallbackContext context*/)
    {
        if (holding)
        {
            rb.isKinematic = false;
            transform.parent = null;
            rb.AddForce(Camera.main.transform.forward * throwForce);
            throwForce = 1000;
            holding = false;
        }
    }
    void Update()
    {
        //dist = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit hit;
        if( Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
        {
            hasplayer = hit.collider.gameObject == this.gameObject;
        }
        else hasplayer = false;

        if (chargingg) throwForce += Time.deltaTime * 200;

        if (Input.GetKeyUp(KeyCode.E)) chargingg = false;
        else Throw();
    } 
}
