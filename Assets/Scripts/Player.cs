using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Rigidbody body;

    public float movementSpeed = 100;
    Vector2 forceToAdd;
    Camera cam;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
        forceToAdd = new Vector2();
        cam = GetComponentInChildren<Camera>();
    }

    public void Update()
    {
        body.AddForce(movementSpeed * forceToAdd.x * Time.deltaTime, 0, movementSpeed * forceToAdd.y * Time.deltaTime);
    }

    public void move(InputAction.CallbackContext context)
    {
        forceToAdd = context.ReadValue<Vector2>();
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

