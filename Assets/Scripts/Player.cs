using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Rigidbody body;

    public float movementSpeed = 100;
    public float horizontalDampening = 5;
    public float dashTime;
    public float dashSpeed;
    public float parryTime;
    public float maxDist = 5;

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
        //body.AddForce(movementSpeed * forceToAdd.x * Time.deltaTime, 0, movementSpeed * forceToAdd.y * Time.deltaTime);
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
        Debug.Log("dash!\n");
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
        Debug.Log("parry!\n");
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

