using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Player : MonoBehaviour
{

    

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
    public float slowSpeedMult = 0.3f;
    bool slowed = false;
    public float horizVelDampMult = 3;
    float timeSinceHolding = 0f;
    public float endDepth = -2000;

    

    int health = 3;

    bool holdingParry = false;

    
    public float dashLinesRate = 45f;
    public float dashLinesSpeed = 45f;

    Vector2 moveDir;

    public Material hurt;
    public Material tapParry;
    public Camera cam;
    public VisualEffect dashLines;
    public Image dieScreen;
    public VideoPlayer vid;
    public Rigidbody body;
    public AudioSource hurtNoise;
    public AudioSource parryNoise;
    public AudioSource tapDashNoise;
    public AudioSource holdDashNoise;
    public AudioSource holdDashEndNoise;
    public AudioSource healNoise;
    public AudioSource dieNoise;

    public void Start()
    {
        //body = GetComponent<Rigidbody>();
        moveDir = new Vector2();
        //cam = GetComponentInChildren<Camera>();
        //vid = GameObject.FindGameObjectWithTag("vidPlayer").GetComponent<VideoPlayer>();
        //dashLines = GetComponentInChildren<VisualEffect>();
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

        //check if at game end
        if (transform.position.y < endDepth)
        {
            SceneManager.LoadScene("end");
        }
    }

    public void takeDamage()
    {
        if (timeSinceLastHurt > hurtCooldown)
        {
            hurtNoise.pitch = (float)PitManager.random.NextDouble() * -0.4f + 1.2f;
            hurtNoise.Play();
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
        SceneManager.LoadScene("game");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hazard"))
        {
            takeDamage();
            Debug.Log("bloop");
        }
        else if (other.CompareTag("holdParry"))
        {
            if (holdingParry)
            {
                parryNoise.pitch = (float)PitManager.random.NextDouble() * -0.4f + 1.2f;
                parryNoise.Play();
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
