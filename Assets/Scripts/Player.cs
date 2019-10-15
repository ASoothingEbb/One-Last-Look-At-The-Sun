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
    public float dashCamShakeMag = 0.2f;
    public float dashCamShakeTime = 0.075f;
    public float parryTime=0.15f;
    public float parryCamShakeMag = 0.2f;
    public float parryCamShakeTime = 0.075f;
    public float maxDist = 5;

    bool holdingDash = false;
    bool holdingParry = false;

    Vector2 forceToAdd;
    Camera cam;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
        forceToAdd = new Vector2();
        cam = GetComponentInChildren<Camera>();
    }

    public void FixedUpdate()
    {
        body.velocity = Vector3.Lerp(body.velocity, new Vector3(forceToAdd.x * movementSpeed, body.velocity.y, forceToAdd.y * movementSpeed), Time.deltaTime * horizontalDampening);
        Vector2 temp = new Vector2(transform.position.x, transform.position.z);
        if (temp.SqrMagnitude() > maxDist*maxDist)
        {
            transform.position = new Vector3( temp.normalized.x * maxDist * 0.99f , transform.position.y, temp.normalized.y*maxDist * 0.99f);
        }
    }

    public void move(InputAction.CallbackContext context)
    {
        forceToAdd = context.ReadValue<Vector2>();
    }

    public void dash(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction && context.performed)
        {
            StartCoroutine(Dash());
            if (forceToAdd.sqrMagnitude > 0.5f)
            {
                StartCoroutine(shakeCamera(dashCamShakeMag, dashCamShakeTime));
            }
        }
    }

    public IEnumerator Dash()
    {
        var time = 0f;
        while(time < dashTime)
        {
            transform.position = new Vector3(forceToAdd.x * dashSpeed * Time.deltaTime, 0, forceToAdd.y * dashSpeed * Time.deltaTime) + transform.position;
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void parry(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.interaction);
        };
    }

    public IEnumerator Parry()
    {
        yield return null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("hazard"))
        {
            //SceneManager.LoadScene("Scenes/game");
            StartCoroutine(shakeCamera(.15f, .4f));
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

