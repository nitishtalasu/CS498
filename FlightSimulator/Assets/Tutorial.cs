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
    public bool isBulletTutorial;
    public float Lift = 0.002f;
    public float AirBrakesEffect = 3f;
    public float DragIncreaseFactor = 0.001f;
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
            displayText.text = "Press left arrow to change the roll";
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
                Vector3 torque = -rollInput * 5f * transform.forward;
                rb.AddTorque(torque);
            }           
        }

        if (isRollRightTutorial)
        {
            displayText.text = "Press right arrow to change the roll";
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
                Vector3 torque = -rollInput * 5f * transform.forward;
                rb.AddTorque(torque);
            }
        }

        if (isPitchUpTutorial)
        {
            displayText.text = "Press up arrow to change the pitch";
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
                Vector3 torque = -pitchInput * 5f * transform.right;
                rb.AddTorque(torque);
            }
        }

        if (isPitchDownTutorial)
        {
            displayText.text = "Press down arrow to change the pitch";
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
                Vector3 torque = -pitchInput * 5f * transform.right;
                rb.AddTorque(torque);
            }
        }

        if (isYawClockwiseTutorial)
        {
            displayText.text = "Press E key to change the yaw";
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

                Vector3 torque = yawInput * 5f * transform.up;
                rb.AddTorque(torque);
            }          
        }

        if (isYawAntiClockwiseTutorial)
        {
            displayText.text = "Press Q key to change the yaw";
            if (Input.GetKey(KeyCode.Q))
            {
                // Debug.Log(transform.localEulerAngles);
                float yawInput = 1f;
                if (transform.localEulerAngles.y >= 89 && transform.localEulerAngles.y < 90)
                {
                    // StartCoroutine(Wait());
                    isYawAntiClockwiseTutorial = false;
                    isAccelerationTutorial = true;
                    transform.position = originalPosition;
                    transform.localEulerAngles = originalLocalEulerAngles;
                }

                Vector3 torque = -yawInput * 5f * transform.up;
                rb.AddTorque(torque);
            }
        }

        if(isAccelerationTutorial)
        {
            displayText.text = "Press Fire1 button to decelerate and Fire2 button to accelerate. Move till the end of the room";
            // Debug.Log(transform.position);
            if (transform.position.x >= 50)
            {
                // StartCoroutine(Wait());
                isAccelerationTutorial = false;
                isBulletTutorial = true;
                transform.position = originalPosition;
                transform.localEulerAngles = originalLocalEulerAngles;
            }
            var localVelocity = transform.InverseTransformDirection(rb.velocity);
            forwardSpeed = Mathf.Max(0, localVelocity.z);
            if (Input.GetButton("Fire2"))
            {
                forwardSpeed += 20.0f;
                //float extraDrag = rb.velocity.magnitude * DragIncreaseFactor;
                //rb.drag = originalDrag + extraDrag;
            }
            if (Input.GetButton("Fire1"))
            {
                float extraDrag = rb.velocity.magnitude * DragIncreaseFactor;
                rb.drag = (originalDrag + extraDrag) * AirBrakesEffect;
                rb.angularDrag = originalAngularDrag * forwardSpeed;
                forwardSpeed -= 0.5f;
            }
            UpdatingVelocity();
            AddingForces();
        }

        if(isBulletTutorial)
        {
            displayText.text = "Press Space button to fire.";
            firing.enabled = true;
            Target.SetActive(true);
            if (Input.GetButton("Jump"))
            {
                firingCount += 1;
            }
            if (firingCount >= 30)
            {
                firing.enabled = false;
                displayText.text = "Nice. Tutorial completed.";
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
        aeroFactor *= aeroFactor;
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
