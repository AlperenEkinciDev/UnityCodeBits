using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMovement : MonoBehaviour
{
    [Header("|---------> Postion Oscilation")]
    [SerializeField] bool positionOscilation;
    [SerializeField] Vector3 positionOscilationAmount;
    [SerializeField] float positionOscilationDuration;
    [SerializeField] [Range(0.0f, 1.0f)] float positionOscilationOffset;
    [SerializeField] bool randomizePositionOscilation;
    private Vector3 posOscStartPos;
    private float posOscStartTime;
    [Header("|---------> Rotation Oscilation")]
    [SerializeField] bool rotationOscilation;
    [SerializeField] Vector3 rotationOscilationAmount;
    [SerializeField] float rotationOscilationDuration;
    [SerializeField][Range(0.0f, 1.0f)] float rotationOscilationOffset;
    [SerializeField] bool randomizeRotationOscilation;
    private Vector3 rotOscStartRot;
    private float rotOscStartTime;
    [Header("|---------> Scale Oscilation")]
    [SerializeField] bool scaleOscilation;
    [SerializeField] Vector3 scaleOscilationAmount;
    [SerializeField] float scaleOscilationDuration;
    [SerializeField] [Range(0.0f, 1.0f)] float scaleOscilationOffset;
    [SerializeField] bool randomizeScaleOscilation;
    private Vector3 scaleOscStartScale;
    private float scaleOscStartTime;
    [Header("|---------> Continuous Movement")]
    [SerializeField] bool movementContinuous;
    [SerializeField] Vector3 movementAmount;
    [Header("|---------> Continuous Rotation")]
    [SerializeField] bool rotationContinuous;
    [SerializeField] Vector3 rotationAmount;
    [Header("|---------> Continuous Scale")]
    [SerializeField] bool scaleContinuous;
    [SerializeField] Vector3 scaleAmount;
    void Start()
    {
        if (positionOscilation)
        {
            posOscStartPos = this.transform.position - positionOscilationAmount / 2f;
            posOscStartTime = Time.time - positionOscilationOffset * positionOscilationDuration;
            if(randomizePositionOscilation) posOscStartTime -= Random.Range(0f, positionOscilationDuration);
            StartCoroutine(OscilatePosition());
        }
        if (rotationOscilation)
        {
            rotOscStartRot = this.transform.localEulerAngles - rotationOscilationAmount / 2f;
            rotOscStartTime = Time.time - rotationOscilationOffset * rotationOscilationDuration;
            if (randomizeRotationOscilation) rotOscStartTime -= Random.Range(0f, rotationOscilationDuration);
            StartCoroutine(OscilateRotation());
        }
        if (scaleOscilation)
        {
            scaleOscStartScale = this.transform.localScale - scaleOscilationAmount / 2f;
            scaleOscStartTime = Time.time - scaleOscilationOffset * scaleOscilationDuration;
            if (randomizeScaleOscilation) scaleOscStartTime -= Random.Range(0f, scaleOscilationDuration);
            StartCoroutine(OscilateScale());
        }
        if (movementContinuous)
        {
            StartCoroutine(ContinuousMovement());
        }
        if (rotationContinuous)
        {
            StartCoroutine(ContinuousRotation());
        }
        if (scaleContinuous)
        {
            StartCoroutine(ContinuousScale());
        }
    }

    IEnumerator OscilatePosition()
    {
        while (true)
        {
            float timeElapsed = Time.time - posOscStartTime;
            float multiplier;
            if (timeElapsed / positionOscilationDuration < 0.5f)
            {
                multiplier = EaseMethods.EaseOutBounce(timeElapsed / (positionOscilationDuration / 2f));
            }
            else if(timeElapsed / positionOscilationDuration > 0.5f && timeElapsed / positionOscilationDuration < 1f)
            {
                multiplier = 1 - EaseMethods.EaseOutBounce(timeElapsed / (positionOscilationDuration / 2f) - 1f);
            }
            else
            {
                multiplier = 0.0f;
                this.transform.position = posOscStartPos;
                posOscStartTime = Time.time;
            }


            this.transform.position = posOscStartPos + multiplier * positionOscilationAmount;

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator OscilateRotation()
    {
        while (true)
        {
            float timeElapsed = Time.time - rotOscStartTime;
            float multiplier;
            if (timeElapsed / rotationOscilationDuration < 0.5f)
            {
                multiplier = EaseMethods.EaseOutBounce(timeElapsed / (rotationOscilationDuration / 2f));
            }
            else if (timeElapsed / rotationOscilationDuration > 0.5f && timeElapsed / rotationOscilationDuration < 1f)
            {
                multiplier = 1 - EaseMethods.EaseOutBounce(timeElapsed / (rotationOscilationDuration / 2f) - 1f);
            }
            else
            {
                multiplier = 0.0f;
                this.transform.eulerAngles = rotOscStartRot;
                rotOscStartTime = Time.time;
            }

            this.transform.eulerAngles = rotOscStartRot + multiplier * rotationOscilationAmount;

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator OscilateScale()
    {
        while (true)
        {
            float timeElapsed = Time.time - scaleOscStartTime;
            float multiplier;
            if (timeElapsed / scaleOscilationDuration < 0.5f)
            {
                multiplier = EaseMethods.EaseOutBounce(timeElapsed / (scaleOscilationDuration / 2f));
            }
            else if (timeElapsed / scaleOscilationDuration > 0.5f && timeElapsed / scaleOscilationDuration < 1f)
            {
                multiplier = 1 - EaseMethods.EaseOutBounce(timeElapsed / (scaleOscilationDuration / 2f) - 1f);
            }
            else
            {
                multiplier = 0.0f;
                this.transform.localScale = scaleOscStartScale;
                scaleOscStartTime = Time.time;
            }

            this.transform.localScale = scaleOscStartScale + multiplier * scaleOscilationAmount;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ContinuousMovement()
    {
        while (true)
        {
            this.transform.Translate(movementAmount * Time.deltaTime);
            posOscStartPos += movementAmount * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator ContinuousRotation()
    {
        while (true)
        {
            this.transform.Rotate(rotationAmount * Time.deltaTime);
            rotOscStartRot += rotationAmount * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator ContinuousScale()
    {
        while (true)
        {
            this.transform.localScale += scaleAmount * Time.deltaTime;
            scaleOscStartScale += scaleAmount * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
