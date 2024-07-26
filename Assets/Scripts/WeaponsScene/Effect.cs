using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] protected string instantiateSFXName;
    [SerializeField] protected string destroySFXName;
    [SerializeField] protected Transform particleTransform;

    protected void PlaySFX(string name)
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.PlaySound(name);
    }
}
