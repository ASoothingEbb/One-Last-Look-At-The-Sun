using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Player : MonoBehaviour
{

    Rigidbody body;

    public float hurtCooldown = 1f;
    float timeSinceLastHurt = 0f;
    public float horizMovementSpeed = 100;
    public float horizVelDamp = 5;
    public float vertVelDamp = 2;
    public float sideSpeedMult = 2f;
    public float fallSpeedMult = 2f;
    public float parryCamShakeMag = 0.2f;
    public float parryCamShakeTime = 0.075f;
    public float maxHorizDist = 5;
    public float fallSpeed = 10f;
    public float parryEffectIntensity = 1f;
    public Material hurt;
    public Material tapParry;
    public float slowSpeedMult = 0.3f;
    bool slowed = false;
    public float horizVelDampMult = 3;
    float timeSinceHolding = 0f;

    public Image dieScreen;
    VideoPlayer vid;

    int health = 3;

    bool holdingParry = false;

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
        vid = GameObject.FindGameObjectWithTag("vidPlayer").GetComponent<VideoPlayer>();
        dashLines = GetComponentInChildren<VisualEffect>();
        tapParry.SetFloat("Vector1_5E361D35", 0);
    }

    public void Update()
    {

        //CALC NEW VELOCITY
        //this code is verbose but its easiest to read and has no performance penalties
        float newX, newY, newZ;
        if (holdingParry)
        {
            if (timeSinceHolding < 0.05f)
            {
                newX = Mathf.Lerp(body.velocity.x, moveDir.x * horizMovementSpeed * sideSpeedMult * 7f, horizVelDamp * Time.deltaTime * horizVelDampMult);
                newY = Mathf.Lerp(body.velocity.y, -fallSpeed * fallSpeedMult, vertVelDamp * Time.deltaTime);
                newZ = Mathf.Lerp(body.velocity.z, moveDir.y * horizMovementSpeed * sideSpeedMult * 7f, horizVelDamp * Time.deltaTime * horizVelDampMult);
            }
            else
            {
                newX = Mathf.Lerp(body.velocity.x, moveDir.x * horizMovementSpeed * sideSpeedMult, horizVelDamp * Time.deltaTime * horizVelDampMult);
                newY = Mathf.Lerp(body.velocity.y, -fallSpeed * fallSpeedMult, vertVelDamp * Time.deltaTime);
                newZ = Mathf.Lerp(body.velocity.z, moveDir.y * horizMovementSpeed * sideSpeedMult, horizVelDamp * Time.deltaTime * horizVelDampMult);
            }
        }
        else if (slowed)
        {
            newX = Mathf.Lerp(body.velocity.x, moveDir.x * horizMovementSpeed * slowSpeedMult, horizVelDamp * Time.deltaTime);
            newY = Mathf.Lerp(body.velocity.y, -fallSpeed * slowSpeedMult, vertVelDamp * Time.deltaTime);
            newZ = Mathf.Lerp(body.velocity.z, moveDir.y * horizMovementSpeed * slowSpeedMult, horizVelDamp * Time.deltaTime);
        }
        else
        {
            newX = Mathf.Lerp(body.velocity.x, moveDir.x * horizMovementSpeed, horizVelDamp * Time.deltaTime);
            newY = Mathf.Lerp(body.velocity.y, -fallSpeed, vertVelDamp * Time.deltaTime);
            newZ = Mathf.Lerp(body.velocity.z, moveDir.y * horizMovementSpeed, horizVelDamp * Time.deltaTime);

        }
        body.velocity = new Vector3(newX, newY, newZ);

        //CHECK OUT OF BOUNDS.
        Vector2 horizPos = new Vector2(transform.position.x, transform.position.z);
        if (horizPos.sqrMagnitude > maxHorizDist * maxHorizDist)
        {
            transform.position = new Vector3(horizPos.normalized.x * maxHorizDist * 0.99f, transform.position.y, horizPos.normalized.y * maxHorizDist * 0.99f);
        }

        timeSinceLastHurt += Time.deltaTime;
        timeSinceHolding += Time.deltaTime;
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
            tapParry.SetFloat("Vector1_5E361D35", parryEffectIntensity);
            timeSinceHolding = 0f;
        }
        else if (context.canceled)
        {
            holdingParry = false;
            tapParry.SetFloat("Vector1_5E361D35", 0);
        }
    }

    public IEnumerator FadeMat(Material mat, string property, float start, float stop, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            mat.SetFloat(property, Mathf.Lerp(start, stop, i / time));
            yield return null;
        }
        mat.SetFloat(property, stop);
    }

    public IEnumerator Die()
    {
        //StartCoroutine(FadeMat(hurt, "Vector1_932E682D", 0, -1, 1));
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            dieScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

    //public IEnumerator ExecuteShortDash()
    //{
    //    StartCoroutine(shakeCamera(parryCamShakeMag, parryCamShakeTime));
    //    Vector3 dir = new Vector3(moveDir.x, 0, moveDir.y);

    //    if (dir.sqrMagnitude < 0.1f)
    //    {
    //        dir.y = -1;
    //    }
    //    else
    //    {
    //        dir.y = -fallSpeed / fallSpeedMult;
    //    }
    //    dashLines.SetFloat("rate", dashLinesRate);
    //    dashLines.SetVector3("velocity", -dir * dashLinesSpeed);
    //    var time = 0f;
    //    while (time < dashTime)
    //    {
    //        body.velocity = new Vector3(dir.normalized.x * sideSpeedMult, dir.normalized.y * fallSpeedMult, dir.normalized.z * sideSpeedMult);
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //    dashLines.SetFloat("rate", 0);
    //    dashLines.SetVector3("velocity", Vector3.zero);
    //}

    //public ienumerator executeshortparry()
    //{
    //    parrying = true;
    //    //vector1_5e361d35
    //    startcoroutine(fademat(tapparry, "vector1_5e361d35", 0, parryeffectintensity, parrytime / 2));
    //    for (float i = 0; i < parrytime / 2; i += time.deltatime)
    //        yield return null;
    //    startcoroutine(fademat(tapparry, "vector1_5e361d35", parryeffectintensity, 0, parrytime / 2));
    //    for (float i = 0; i < parrytime / 2; i += time.deltatime)
    //        yield return null;
    //    parrying = false;
    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hazard"))
        {
            takeDamage();
        }
        else if (other.CompareTag("holdParry"))
        {
            if (holdingParry)
            {
                //play sound
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
        else if (other.CompareTag("bounce"))
        {
            body.velocity = -body.velocity * 0.5f;
        }
        else if (other.CompareTag("slow"))
        {
            slowed = true;
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("cliff"))
        {
            health = 3;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("vid"))
        {
            vid.Stop();
        }
        else if (other.CompareTag("slow"))
        {
            slowed = false;
        }
    }

    public IEnumerator shakeCamera(float mag, float dur)
    {
        Transform camTrans = cam.transform;
        var time = 0f;

        while (time < dur)
        {
            camTrans.localPosition = new Vector3(Random.Range(-1f, 1f) * mag, 0, Random.Range(-1f, 1f) * mag);
            time += Time.deltaTime;
            yield return null;
        }

        camTrans.localPosition = Vector3.zero;
    }

}
