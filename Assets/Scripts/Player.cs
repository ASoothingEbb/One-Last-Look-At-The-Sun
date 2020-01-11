using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Experimental.VFX;

public class Player : MonoBehaviour
{

    Rigidbody body;

    public float hurtCooldown = 1f;
    float timeSinceLastHurt = 0f;
    public float movementSpeed = 100;
    public float horizontalDampening = 5;
    public float dashTime=0.15f;
    public float dashSideSpeed=2f;
    public float dashDownSpeed = 2f;
    public float parryTime=0.15f;
    public float parryCooldown = 2f;
    float timeSinceLastParry = 0  ;
    public float parryCamShakeMag = 0.2f;
    public float parryCamShakeTime = 0.075f;
    public float maxDist = 5;
    public float maxFallSpeed = 10f;
    public float parryEffectIntensity = 1f;
    public Material hurt;
    public Material tapParry;

    public Image dieScreen;
    VideoPlayer vid;

    int health = 3;

    bool holdingParry = false;
    bool parrying = false;

    VisualEffect dashLines;
    public float dashLinesRate = 45f;
    public float dashLinesSpeed = 45f;

    Vector2 moveDir;
    Camera cam;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
        moveDir = new Vector2();
        cam = GetComponentInChildren<Camera>();
        vid = GameObject.FindGameObjectWithTag("vid").GetComponent<VideoPlayer>();
        dashLines = GetComponentInChildren<VisualEffect>();

    }

    public void Update()
    {
        body.velocity = Vector3.Lerp(body.velocity, new Vector3(moveDir.x * movementSpeed, body.velocity.y, moveDir.y * movementSpeed), Time.deltaTime * horizontalDampening);
        body.velocity = new Vector3(body.velocity.x, Mathf.Max(body.velocity.y, - maxFallSpeed), body.velocity.z);
        Vector2 temp = new Vector2(transform.position.x, transform.position.z);
        if (temp.SqrMagnitude() > maxDist*maxDist)
        {
            transform.position = new Vector3( temp.normalized.x * maxDist * 0.99f , transform.position.y, temp.normalized.y*maxDist * 0.99f);
        }

        if(holdingParry && timeSinceLastParry > parryCooldown)
        {
            timeSinceLastParry = 0;
            StartCoroutine(ExecuteShortParry());
            StartCoroutine(ExecuteShortDash());

        }

        timeSinceLastParry += Time.deltaTime;
        timeSinceLastHurt += Time.deltaTime;
        hurt.SetFloat("Vector1_932E682D", health);
    }

    public void takeDamage()
    {
        if (timeSinceLastHurt > hurtCooldown)
        {
            timeSinceLastHurt = 0;
            health -= 1;
            StartCoroutine(shakeCamera(.4f, .4f));
            if (health < 1)
            {
                StartCoroutine(shakeCamera(0.4f, 1f));
                StartCoroutine(Die());
            }
        }

    }

    public void move(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    public void parry(InputAction.CallbackContext context)
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

    public IEnumerator FadeMat(Material mat, string property, float start, float stop, float time)
    {
        for(float i = 0; i < time; i += Time.deltaTime)
        {
            mat.SetFloat(property, Mathf.Lerp(start, stop, i / time));
            yield return null;
        }
        mat.SetFloat(property, stop);
    }

    public IEnumerator Die()
    {
        StartCoroutine(FadeMat(hurt, "Vector1_932E682D", 0, -1, 1));
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            dieScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

    public IEnumerator ExecuteShortDash()
    {
        StartCoroutine(shakeCamera(parryCamShakeMag, parryCamShakeTime));
        Vector3 dir = new Vector3(moveDir.x, 0, moveDir.y);

        if(dir.sqrMagnitude < 0.1f)
        {
            dir.y = -1;
        }
        dashLines.SetFloat("rate", dashLinesRate);
        dashLines.SetVector3("velocity", -dir * dashLinesSpeed);
        var time = 0f;
        while(time < dashTime)
        {
            transform.position = new Vector3(dir.normalized.x * dashSideSpeed * Time.deltaTime, dir.normalized.y * dashDownSpeed * Time.deltaTime, dir.normalized.z * dashSideSpeed * Time.deltaTime) + transform.position;
            time += Time.deltaTime;
            yield return null;
        }
        dashLines.SetFloat("rate", 0);
        dashLines.SetVector3("velocity", Vector3.zero);
    } 

    public IEnumerator ExecuteShortParry()
    {
        parrying = true;
        //Vector1_5E361D35
        StartCoroutine(FadeMat(tapParry, "Vector1_5E361D35", 0, parryEffectIntensity, parryTime / 2));
        for (float i = 0; i < parryTime / 2; i += Time.deltaTime)
            yield return null;
        StartCoroutine(FadeMat(tapParry, "Vector1_5E361D35", parryEffectIntensity, 0, parryTime / 2));
        for (float i = 0; i < parryTime / 2; i += Time.deltaTime)
            yield return null;
        parrying = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hazard"))
        {
            takeDamage();
        }
        else if (other.CompareTag("tapParry"))
        {
            if (parrying)
            {

            }
            else
            {
                takeDamage();
            }
        }
        else if (other.CompareTag("holdParry"))
        {
            if (holdingParry)
            {

            }
            else
            {
                takeDamage();
            }
        }
        else if (other.CompareTag("vid"))
        {
            vid.url = Application.dataPath + "/Videos/" + other.name + ".mp4";
            vid.Play();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("vid"))
        {
            vid.Stop();
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

