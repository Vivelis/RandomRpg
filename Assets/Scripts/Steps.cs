using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class Steps : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepsClips = null;
    [SerializeField] private float timeBetweenSteps = 0.5f;
    [SerializeField] private float minimumVelocity = 0.1f;
    private AudioSource audioSource;
    private CharacterController characterController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        audioSource.loop = false;
        StartCoroutine(PlaySteps());
    }

    private IEnumerator PlaySteps()
    {
        while (true)
        {
            yield return new WaitUntil(() => characterController.velocity.magnitude > minimumVelocity);
            Debug.Log(characterController.velocity.magnitude);
            PlayRandomSteps();
            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }

    private void PlayRandomSteps()
    {
        audioSource.clip = stepsClips[UnityEngine.Random.Range(0, stepsClips.Length)];
        audioSource.Play();
    }
}
