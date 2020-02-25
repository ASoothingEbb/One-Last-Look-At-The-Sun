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
    float timeSinceParryEnded = 0f;
    float timeSinceHolding = 0f;
    public float parryPityWindow = 0.3f;
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

    public bool accelerated;
    public float acceleratedMult = 1.25f;
    public float initDashTime = 0.05f;
    public float initDashMult = 7;

    public int health = 3;

    public float parryPitchMaxTime = 1f;
    public int maxParryPitch =  5;
    public float parryPitchSpacing = 0.1f;
    int currentParryPitch = 0;
    float timeSinceLastParryHit = 0f;

    bool holdingParry = false;

    Vector2 moveDir;

    public Material hurt;
    public Material tapParry;
    public Camera cam;
    public Image dieScreen;
    public VideoPlayer vid;
    public Rigidbody body;
    public AudioSource hurtNoise;
    public AudioSource parryNoise;
    public AudioSource windNoise;
    public AudioSource healNoise;
    public AudioSource accelNoise;

    public void Start()
    {
        moveDir = new Vector2();
        tapParry.SetFloat("Vector1_5E361D35", 0);
    }

    public void Update()
    {

        //CALC NEW VELOCITY
        float newX = moveDir.x * horizMovementSpeed;
        float newY = -fallSpeed;
        float newZ = moveDir.y * horizMovementSpeed; 
        if (holdingParry)
        {
          
            newX *= sideSpeedMult;
            newY *= fallSpeedMult;
            newZ *= sideSpeedMult;
            if (timeSinceHolding < initDashTime)
            {
                newX *= initDashMult;
                newZ *= initDashMult;
            }
        }
        else if (slowed)
        {
            newX *= slowSpeedMult;
            newY *= slowSpeedMult;
            newZ *= slowSpeedMult;
        }

        if (accelerated)
        {
            newX *= acceleratedMult;
            newY *= acceleratedMult;
            newZ *= acceleratedMult;
        }

        newX = Mathf.Lerp(body.velocity.x, newX, horizVelDamp * Time.deltaTime);
        newY = Mathf.Lerp(body.velocity.y, newY, vertVelDamp * Time.deltaTime);
        newZ = Mathf.Lerp(body.velocity.z, newZ, horizVelDamp * Time.deltaTime);

        body.velocity = new Vector3(newX, newY, newZ);

        //CHECK OUT OF BOUNDS.
        Vector2 horizPos = new Vector2(transform.position.x, transform.position.z);
        if (horizPos.sqrMagnitude > maxHorizDist * maxHorizDist)
        {
            transform.position = new Vector3(horizPos.normalized.x * maxHorizDist * 0.99f, transform.position.y, horizPos.normalized.y * maxHorizDist * 0.99f);
        }

        timeSinceLastHurt += Time.deltaTime;
        timeSinceHolding += Time.deltaTime;
        timeSinceParryEnded += Time.deltaTime;
        timeSinceLastParryHit += Time.deltaTime;
        hurt.SetFloat("Vector1_932E682D", health);

        //check if at game end
        if (transform.position.y < PitManager.max_depth)
        {
            StartCoroutine(FadeOut(1,1,1, "end", 1));
        }

        //mo speed mo wind
        windNoise.pitch = Mathf.Clamp( ( ( Mathf.Abs(body.velocity.y) + Mathf.Abs(body.velocity.x) + Mathf.Abs(body.velocity.z)) / (fallSpeed * fallSpeedMult * acceleratedMult + horizMovementSpeed * sideSpeedMult)) * 0.75f + 0.5f, 0.5f, 2.5f); 
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
                StartCoroutine(FadeOut(0,0,0, "death", 0.2f));
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
            timeSinceParryEnded = 0;
            tapParry.SetFloat("Vector1_5E361D35", 0);
        }
    }

    public IEnumerator FadeOut(float r, float g, float b, string sceneToLoad, float time)
    {
        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            dieScreen.color = new Color(r, g, b, i/time);
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hazard"))
        {
            takeDamage();
        }
        else if (other.CompareTag("holdParry"))
        {
            if (holdingParry || timeSinceParryEnded < parryPityWindow)
            {
                if(timeSinceLastParryHit < parryPitchMaxTime)
                {
                    currentParryPitch = Mathf.Min(currentParryPitch+1, maxParryPitch);
                }
                else
                {
                    currentParryPitch = 0;
                }
                timeSinceLastParryHit = 0;
                parryNoise.pitch = 1 + currentParryPitch * parryPitchSpacing;
                parryNoise.Play();
            }
            else
            {
                takeDamage();
            }
        }
        else if (other.CompareTag("accel"))
        {
            accelerated = true;
            accelNoise.Play();
        }
        else if (other.CompareTag("vid"))
        {
            vid.url = Application.dataPath + "/Videos/" + other.name + ".mp4";
            vid.Play();
        }
        else if (other.CompareTag("bounce"))
        {
            body.velocity = -body.velocity * 5f;
        }
        else if (other.CompareTag("slow"))
        {
            slowed = true;
        } else if (other.CompareTag("heal"))
        {
            health = 3;
            accelerated = false;
            healNoise.Play();
        }
    }

    public void OnCollisionEnter(Collision col)
    {
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
