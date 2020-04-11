using UnityEngine;

public class NewFly : MonoBehaviour
{
    public float MaxEnginePower = 40f;        
    public float Lift = 0.002f;               
    public float ZeroLiftSpeed = 300;         
    public float RollEffect = 1f;             
    public float PitchEffect = 1f;            
    public float YawEffect = 0.2f;            
    public float BankedTurnEffect = 0.5f;     
    public float AerodynamicEffect = 0.02f;   
    public float AutoTurnPitch = 0.5f;        
    public float AutoRollLevel = 0.2f;        
    public float AutoPitchLevel = 0.2f;       
    public float AirBrakesEffect = 3f;        
    public float ThrottleChangeSpeed = 0.3f;  
    public float DragIncreaseFactor = 0.001f;
    public float MaximumSpeed = 120.0f;
    public float MinimumSpeed = 30.0f;
    public float SpeedEffect = 5.0f;
                 
    private float throttle;                   
    private bool airBrakes;                   
    private float forwardSpeed;               
    private float enginePower;
    private float rollAngle;
    private float pitchAngle;
    private float rollInput;
    private float pitchInput;
    private float yawInput;
    private float throttleInput;

    private Rigidbody rb;
    private float originalDrag;
    private float originalAngularDrag;
    private float aeroFactor;
    private float bankedTurnAmount;


    public float maxRollAngle = 80;
    public float maxPitchAngle = 80;
    public float camForwardDistance = 20;
    public float camRightDistance = 20;
    public float camUpDistance = 20;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Store original drag settings, these are modified during flight.
        originalDrag = rb.drag;
        originalAngularDrag = rb.angularDrag;
        yawInput = 0;

        //Camera main = Camera.main;
        //Debug.Log("before transform: " + main.transform.position);
        //main.transform.localPosition = transform.position - transform.forward * camForwardDistance - Vector3.right * camRightDistance + transform.up * camUpDistance;
        //Debug.Log("after transform: " + main.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Menu.isGameEnded)
        {
            return;
        }

        rollInput = Input.GetAxis("Horizontal");
        pitchInput = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            yawInput += YawEffect;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            yawInput -= YawEffect;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            yawInput = 0;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            yawInput = 0;
        }

        airBrakes = Input.GetButton("Fire1");
        throttleInput = airBrakes ? -1 : 1;

        CalculateAngles();

        AutoLevel();

        // Getting the engine power.
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        forwardSpeed = Mathf.Max(0, localVelocity.z);
        if (Input.GetButton("Fire2"))
        {
            forwardSpeed += SpeedEffect;
        }
        throttle = Mathf.Clamp01(throttle + throttleInput * Time.deltaTime * ThrottleChangeSpeed);
        enginePower = throttle * MaxEnginePower;

        // Finding the drag on the plane.
        float extraDrag = rb.velocity.magnitude * DragIncreaseFactor;
        if (airBrakes)
        {
            rb.drag = (originalDrag + extraDrag) * AirBrakesEffect;
        }
        else
        {
            rb.drag = originalDrag + extraDrag;
        }
        rb.angularDrag = originalAngularDrag * forwardSpeed;

        UpdatingVelocity();

        AddingForces();

        AddingTorque();

        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        // Debug.Log("terrain height :" + terrainHeight + "\t and transform y: " + transform.position.y);
        if (terrainHeight >= transform.position.y)
        {
            // transform.position = new Vector3(transform.position.x, terrainHeight + 100.0f, transform.position.z);
            // transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, terrainHeight + 100.0f, transform.position.z), 10);
            FindObjectOfType<Menu>().GameOver();
        }
    }

    private void CalculateAngles()
    {    
        Vector3 flatForward = transform.forward;
        flatForward.y = 0;
        if (flatForward.sqrMagnitude <= 0)
        {
            return;
        }

        flatForward.Normalize();
        var localFlatForward = transform.InverseTransformDirection(flatForward);
        pitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z);
        if (pitchAngle > maxPitchAngle)
        {
            pitchAngle = maxPitchAngle;
        }
        var flatRight = Vector3.Cross(Vector3.up, flatForward);
        var localFlatRight = transform.InverseTransformDirection(flatRight);
        rollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x);
        if (rollAngle > maxRollAngle)
        {
            rollAngle = maxRollAngle;
        }
    }

    private void AutoLevel()
    {
        bankedTurnAmount = Mathf.Sin(rollAngle);
        if (rollInput == 0f)
        {
            rollInput = -rollAngle * AutoRollLevel;
        }
        
        if (pitchInput == 0f)
        {
            pitchInput = -pitchAngle * AutoPitchLevel;
            pitchInput -= Mathf.Abs(bankedTurnAmount * bankedTurnAmount * AutoTurnPitch);
        }
    }

    private void UpdatingVelocity()
    {
        if (rb.velocity.magnitude <= 0)
        {
            return;
        }

        aeroFactor = Vector3.Dot(transform.forward, rb.velocity.normalized);
        aeroFactor *= aeroFactor;
        var velocity = Vector3.Lerp(rb.velocity, transform.forward * forwardSpeed,
                                       aeroFactor * forwardSpeed * AerodynamicEffect * Time.deltaTime);

        float speed = Vector3.Magnitude(velocity);
        if (speed > MaximumSpeed)
        {
            float speedDiff = speed - MaximumSpeed;
            Vector3 normalisedVelocity = velocity.normalized;
            Vector3 velocityDiff = normalisedVelocity * speedDiff;
            velocity -= velocityDiff;
        }
        else if (speed < MinimumSpeed)
        {
            float speedDiff = speed - MinimumSpeed;
            Vector3 normalisedVelocity = velocity.normalized;
            Vector3 velocityDiff = normalisedVelocity * speedDiff;
            velocity -= velocityDiff;
        }

        rb.velocity = velocity;
        rb.rotation = Quaternion.Slerp(rb.rotation,
                                              Quaternion.LookRotation(rb.velocity, transform.up),
                                              AerodynamicEffect * Time.deltaTime);
    }

    private void AddingForces()
    {
        var forces = Vector3.zero;
        var velocity = rb.velocity;
        forces += enginePower * transform.forward;
        var liftDirection = Vector3.Cross(velocity, transform.right).normalized;
        var liftPower = forwardSpeed * forwardSpeed * Lift * aeroFactor;
        forces += liftPower * liftDirection;
        float speed = Vector3.Magnitude(velocity);
        // Debug.Log("speed :" + speed);
        if (speed > MaximumSpeed)
        {
            // Debug.Log("Max before applying force :" + forces);
            float brakeSpeed = speed - MaximumSpeed;  
            Vector3 normalisedVelocity = velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  
            forces -= brakeVelocity;
            // Debug.Log("Max after applying force :" + forces);
        }
        rb.AddForce(forces);
    }

    private void AddingTorque()
    {
        var torque = Vector3.zero;
        torque += -pitchInput * PitchEffect * transform.right;
        torque += -yawInput * YawEffect * transform.up;
        torque += -rollInput * RollEffect * transform.forward;
        torque += bankedTurnAmount * BankedTurnEffect * transform.up;
        rb.AddTorque(torque * forwardSpeed * aeroFactor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<Menu>().GameOver();
    }
}
