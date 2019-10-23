﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
    public float maxFallSpeed = 10f;
    public float parrySpeedBoost = 5f;
    public float parryMovePenalty = 0.5f;

    public Material hurt;
    public Material tapParry;
    public Material holdParry;

    public Image dieScreen;

    int health = 3;

    bool holdingParry = false;
    bool tappedParry = true;

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
        body.velocity = Vector3.Lerp(body.velocity, new Vector3(moveDir.x * movementSpeed * (holdingParry ? parryMovePenalty : 1), body.velocity.y, moveDir.y * movementSpeed * (holdingParry ? parryMovePenalty : 1)), Time.deltaTime * horizontalDampening);
        body.velocity = new Vector3(body.velocity.x, Mathf.Max(body.velocity.y, - maxFallSpeed + (holdingParry ? -parrySpeedBoost : 0)), body.velocity.z);
        Vector2 temp = new Vector2(transform.position.x, transform.position.z);
        if (temp.SqrMagnitude() > maxDist*maxDist)
        {
            transform.position = new Vector3( temp.normalized.x * maxDist * 0.99f , transform.position.y, temp.normalized.y*maxDist * 0.99f);
        }
        hurt.SetFloat("Vector1_932E682D", health);
        if (health < 1)
        {
            StartCoroutine(Die());
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
            if (moveDir.sqrMagnitude > 0.05)
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
                StartCoroutine(FadeMat(holdParry, "Vector1_5E361D35", 10, 1.5f, 0.4f));
            }
            else if (context.canceled)
            {
                holdingParry = false;
                StartCoroutine(FadeMat(holdParry, "Vector1_5E361D35", 1.5f, 10, 0.4f));
            }
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
        SceneManager.LoadScene(0);
    }

    public IEnumerator ExecuteShortDash()
    {
        StartCoroutine(shakeCamera((moveDir.x + moveDir.y) / 10, dashCamShakeTime));
        var time = 0f;
        Vector2 dir = new Vector2(moveDir.x, moveDir.y);
        while(time < dashTime)
        {
            transform.position = new Vector3(dir.normalized.x * dashSpeed * Time.deltaTime, 0, dir.normalized.y * dashSpeed * Time.deltaTime) + transform.position;
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator ExecuteShortParry()
    {
        tappedParry = true;
        //Vector1_5E361D35
        StartCoroutine(FadeMat(tapParry, "Vector1_5E361D35", 0, 1, parryTime / 2));
        for (float i = 0; i < parryTime / 2; i += Time.deltaTime)
            yield return null;
        StartCoroutine(FadeMat(tapParry, "Vector1_5E361D35", 1, 0, parryTime / 2));
        for (float i = 0; i < parryTime / 2; i += Time.deltaTime)
            yield return null;
        tappedParry = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("hazard"))
        {
            StartCoroutine(shakeCamera(.4f, .4f));
            health -= 1;
        }
        else if (other.CompareTag("tapParry"))
        {
            if (tappedParry)
            {

            }
            else
            {
                StartCoroutine(shakeCamera(.4f, .4f));
                health = -1;
            }
        }
        else if (other.CompareTag("holdParry"))
        {
            if (holdingParry)
            {

            }
            else
            {
                StartCoroutine(shakeCamera(.4f, .4f));
                health -= 1;
            }
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

