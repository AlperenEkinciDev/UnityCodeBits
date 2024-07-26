using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public void LoadSceneSequence(int i)
    {

        SceneManagerObject sceneManagerObject = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagerObject>();

        sceneManagerObject.LoadSceneSequence(i);
    }
    public void PlaySound(string name)
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        audioManager.PlaySound(name);
    }
 }
