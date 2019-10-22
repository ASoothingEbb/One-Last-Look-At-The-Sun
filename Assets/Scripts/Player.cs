using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{

    Rigidbody body;

    public float movementSpeed = 100;
    public float horizontalDampening = 5;
    public float dashTime=0.15f;
    public float dashSpeed=2f;
    public float dashCooldown = 2f;
    float timeSinceLastDash = 0;
    public float dashCamShakeMag = 0.2f;
    public float dashCamShakeTime = 0.075f;
    public float parryTime=0.15f;
    public float parryCooldown = 2f;
    float timeSinceLastParry;
    public float parryCamShakeMag = 0.2f;
    public float parryCamShakeTime = 0.075f;
    public float maxDist = 5;

    int health = 3;

    bool holdingParry = false;

    Vector2 moveDir;
    Camera cam;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
        moveDir = new Vector2();
        cam = GetComponentInChildren<Camera>();
    }

    public void FixedUpdate()
    {
        body.velocity = Vector3.Lerp(body.velocity, new Vector3(moveDir.x * movementSpeed, body.velocity.y, moveDir.y * movementSpeed), Time.deltaTime * horizontalDampening);
        Vector2 temp = new Vector2(transform.position.x, transform.position.z);
        if (temp.SqrMagnitude() > maxDist*maxDist)
        {
            transform.position = new Vector3( temp.normalized.x * maxDist * 0.99f , transform.position.y, temp.normalized.y*maxDist * 0.99f);
        }
    }

    public void move(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    public void magic(InputAction.CallbackContext context)
    {
        Debug.Log(context.interaction + " - " + context.phase);
        if (context.interaction is TapInteraction && context.performed)
        {
            //utv niceshot rally trakcs automaton starfish anyrocketpass4topper qilinhornscomp3v3 ftlwheels lunchbox
            if (moveDir.sqrMagnitude > 0.1)
            {
                StartCoroutine(ExecuteShortDash());
            }
            else
            {
                StartCoroutine(ExecuteShortParry());
            }
        }
        else if(context.interaction is HoldInteraction)
        {
            if (context.started)
            {
                holdingParry = true;
            }
            else if (context.canceled)
            {
                holdingParry = false;
            }
        }
    }

    public IEnumerator ExecuteShortDash()
    {
        StartCoroutine(shakeCamera((moveDir.x + moveDir.y) / 10, dashCamShakeTime));
        var time = 0f;
        while(time < dashTime)
        {
            transform.position = new Vector3(moveDir.x * dashSpeed * Time.deltaTime, 0, moveDir.y * dashSpeed * Time.deltaTime) + transform.position;
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator ExecuteShortParry()
    {
        yield return null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("hazard"))
        {
            StartCoroutine(shakeCamera(.4f, .4f));
        }
    }

    public IEnumerator shakeCamera(float mag, float dur)
    {
        Transform camTrans = cam.transform;
        var time = 0f;

        while(time < dur)
        {
            camTrans.localPosition = new Vector3(Random.Range(-1f, 1f) * mag, 0, Random.Range(-1f, 1f) * mag);
            time += Time.deltaTime;
            yield return null;
        }

        camTrans.localPosition = Vector3.zero;
    }

}

