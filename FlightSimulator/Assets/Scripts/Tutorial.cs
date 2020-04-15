using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public GameObject TutorialPanel;
    public GameObject Target;
    public bool isTutorialStarted;
    public bool isRollLeftTutorial;
    public bool isRollRightTutorial;
    public bool isPitchUpTutorial;
    public bool isPitchDownTutorial;
    public bool isYawClockwiseTutorial;
    public bool isYawAntiClockwiseTutorial;
    public bool isAccelerationTutorial;
    public bool isDecelerationTutorial;
    public bool isBulletTutorial;
    public float Lift = 0.002f;
    public float AirBrakesEffect = 0.3f;
    public float DragIncreaseFactor = 0.001f;
    public float directionSpeed = 15f;

    private Text displayText;
    private Rigidbody rb;
    private float aeroFactor;
    private float forwardSpeed;
    private float originalDrag;
    private float originalAngularDrag;
    private Firing firing;
    private int firingCount;
    private Vector3 originalPosition;
    private Vector3 originalLocalEulerAngles;


    // Start is called before the first frame update
    void Start()
    {
        isTutorialStarted = false;
        isRollLeftTutorial = false;
        isRollRightTutorial = false;
        isPitchUpTutorial = false;
        isPitchDownTutorial = false;
        isYawClockwiseTutorial = false;
        isYawAntiClockwiseTutorial = false;
        isAccelerationTutorial = false;
        isBulletTutorial = false;
        displayText = GameObject.FindGameObjectWithTag("TutorialText").GetComponent<Text>();
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        originalAngularDrag = rb.angularDrag;
        firing = GetComponentInChildren<Firing>();
        firing.enabled = false;
        firingCount = 0;
        originalPosition = transform.position;
        originalLocalEulerAngles = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTutorialStarted)
        {
            displayText.text = "Press S to start the tutorial. In tutorial follow the instructions on the screen. Press Esc to go back to Main menu.";
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (!isTutorialStarted)
            {
                isTutorialStarted = true;
                isRollLeftTutorial = true;
            }
        }

        if (isRollLeftTutorial)
        {
            displayText.text = "keep pressing left arrow to change the roll";
            float rollInput = Input.GetAxis("Horizontal");
            // Debug.Log(transform.rotation);
            if (transform.localEulerAngles.z >= 10)
            {
                // StartCoroutine(Wait());
                isRollLeftTutorial = false;
                isRollRightTutorial = true;
            }
            if (rollInput < 0)
            {
                Vector3 torque = -rollInput * directionSpeed * transform.forward;
                rb.AddTorque(torque);
            }           
        }

        if (isRollRightTutorial)
        {
            displayText.text = "keep pressing right arrow to change the roll";
            float rollInput = Input.GetAxis("Horizontal");
            //Debug.Log(transform.localEulerAngles);
            if (transform.localEulerAngles.z >= 350 && transform.localEulerAngles.z <= 360)
            {
                // StartCoroutine(Wait());
                isRollRightTutorial = false;
                isPitchUpTutorial = true;
            }
            if (rollInput > 0)
            {
                Vector3 torque = -rollInput * directionSpeed * transform.forward;
                rb.AddTorque(torque);
            }
        }

        if (isPitchUpTutorial)
        {
            displayText.text = "keep pressing up arrow to change the pitch";
            float pitchInput = Input.GetAxis("Vertical");
            // Debug.Log(transform.localEulerAngles);
            if (transform.localEulerAngles.x > 350 && transform.localEulerAngles.x < 355)
            {
                // StartCoroutine(Wait());
                isPitchUpTutorial = false;
                isPitchDownTutorial = true;
            }
            if (pitchInput > 0)
            {
                Vector3 torque = -pitchInput * directionSpeed * transform.right;
                rb.AddTorque(torque);
            }
        }

        if (isPitchDownTutorial)
        {
            displayText.text = "keep pressing down arrow to change the pitch";
            float pitchInput = Input.GetAxis("Vertical");
            // Debug.Log(transform.localEulerAngles);
            if (transform.localEulerAngles.x > 0 && transform.localEulerAngles.x < 3)
            {
                // StartCoroutine(Wait());
                isPitchDownTutorial = false;
                isYawClockwiseTutorial = true;
            }
            if (pitchInput < 0)
            {
                Vector3 torque = -pitchInput * directionSpeed * transform.right;
                rb.AddTorque(torque);
            }
        }

        if (isYawClockwiseTutorial)
        {
            displayText.text = "keep pressing E key to change the yaw";
            if(Input.GetKey(KeyCode.E))
            {
                Debug.Log(transform.localEulerAngles);
                float yawInput = 1f;
                if (transform.localEulerAngles.y > 93)
                {
                    // StartCoroutine(Wait());
                    isYawClockwiseTutorial = false;
                    isYawAntiClockwiseTutorial = true;
                }

                Vector3 torque = yawInput * directionSpeed * transform.up;
                rb.AddTorque(torque);
            }          
        }

        if (isYawAntiClockwiseTutorial)
        {
            displayText.text = "keep pressing Q key to change the yaw";
            if (Input.GetKey(KeyCode.Q))
            {
                // Debug.Log(transform.localEulerAngles);
                float yawInput = 1f;
                if (transform.localEulerAngles.y >= 89 && transform.localEulerAngles.y < 90)
                {
                    // StartCoroutine(Wait());
                    isYawAntiClockwiseTutorial = false;
                    isAccelerationTutorial = true;
                    rb.velocity = new Vector3(5, 0, 0);
                    rb.drag = 0.25f;
                    transform.position = originalPosition;
                    transform.localEulerAngles = originalLocalEulerAngles;
                }

                Vector3 torque = -yawInput * directionSpeed * transform.up;
                rb.AddTorque(torque);
            }
        }

        if(isAccelerationTutorial)
        {
            displayText.text = "keep pressing Z to accelerate.";
            // Debug.Log(transform.position);
            if (transform.position.x >= 50)
            {
                // StartCoroutine(Wait());
                rb.velocity = new Vector3(5, 0, 0);
                transform.position = originalPosition;
                transform.localEulerAngles = originalLocalEulerAngles;
            }
               
            if (Input.GetKey(KeyCode.Z))
            {
                if (rb.velocity.magnitude > 13)
                {
                    Debug.Log("Change to deceleration");
                    isAccelerationTutorial = false;
                    isDecelerationTutorial = true;
                    rb.velocity = new Vector3(15, 0, 0);
                    rb.drag = 0;
                    transform.position = originalPosition;
                    transform.localEulerAngles = originalLocalEulerAngles;
                }
                rb.velocity += Vector3.right * 0.25f;
            }
            //UpdatingVelocity();
            //AddingForces();
        }

        if (isDecelerationTutorial)
        {
            displayText.text = "keep pressing X to decelerate.";
            // Debug.Log(transform.position);
            if (transform.position.x >= 50)
            {
                // StartCoroutine(Wait());
                transform.position = originalPosition;
                transform.localEulerAngles = originalLocalEulerAngles;
            }

            var localVelocity = transform.InverseTransformDirection(rb.velocity);
            forwardSpeed = Mathf.Max(0, localVelocity.x);
            if (Input.GetKey(KeyCode.X))
            {
                if (rb.velocity.magnitude < 10)
                {
                    Debug.Log("Change to bullet");
                    isDecelerationTutorial = false;
                    isBulletTutorial = true;
                    rb.velocity = new Vector3(0, 0, 0);
                    transform.position = originalPosition;
                    transform.localEulerAngles = originalLocalEulerAngles;
                }

                float extraDrag = rb.velocity.magnitude * DragIncreaseFactor;
                rb.drag = (originalDrag + extraDrag) * AirBrakesEffect;
                rb.angularDrag = originalAngularDrag * forwardSpeed;
            }
            //UpdatingVelocity();
            //AddingForces();
        }

        if (isBulletTutorial)
        {
            displayText.text = "keep pressing Space button to fire.";
            firing.enabled = true;
            Target.SetActive(true);
            if (Input.GetButton("Jump"))
            {
                firingCount += 1;
            }
            if (firingCount >= 30)
            {
                firing.enabled = false;
                displayText.text = "Nice. Tutorial completed. Press Esc to return Main menu.";
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void UpdatingVelocity()
    {
        aeroFactor = Vector3.Dot(transform.forward, rb.velocity.normalized);
        // aeroFactor *= aeroFactor;
        var velocity = Vector3.Lerp(rb.velocity, transform.forward * forwardSpeed,
                                       aeroFactor * forwardSpeed * 0.02f * Time.deltaTime);

        float speed = Vector3.Magnitude(velocity);
        // Debug.Log(speed);
        if (speed > 20)
        {
            float speedDiff = speed - 20;
            Vector3 normalisedVelocity = velocity.normalized;
            Vector3 velocityDiff = normalisedVelocity * speedDiff;
            velocity -= velocityDiff;
        }
        else if (speed < 2)
        {
            float speedDiff = speed - 2;
            Vector3 normalisedVelocity = velocity.normalized;
            Vector3 velocityDiff = normalisedVelocity * speedDiff;
            velocity -= velocityDiff;
        }

        rb.velocity = velocity;
        rb.rotation = Quaternion.Slerp(rb.rotation,
                                              Quaternion.LookRotation(rb.velocity, transform.up),
                                              20f * Time.deltaTime);
    }

    private void AddingForces()
    {
        var forces = Vector3.zero;
        var velocity = rb.velocity;
        forces += 400 * transform.forward;
        var liftDirection = Vector3.Cross(velocity, transform.right).normalized;
        var liftPower = forwardSpeed * forwardSpeed * Lift * aeroFactor;
        forces += liftPower * liftDirection;
        float speed = Vector3.Magnitude(velocity);
        rb.AddForce(forces);
    }

    IEnumerator Wait()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
