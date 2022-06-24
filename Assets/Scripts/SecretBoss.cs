using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretBoss : MonoBehaviour
{
    [SerializeField] Button yesButton, noButton;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource, secretBackSource;
    [SerializeField] AudioClip wanaClip, noClip, yesClip;
    [SerializeField] GameObject secretFinish, passCanvas;

    void Start()
    {
        passCanvas.SetActive(false);
        yesButton.onClick.AddListener(SayYes);
        noButton.onClick.AddListener(SayNo);
        audioSource.PlayOneShot(wanaClip);
        StartCoroutine(WanaQuestionEnable());
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    IEnumerator WanaQuestionEnable()
    {
        yield return new WaitForSeconds(8f);
        animator.SetBool("wana", true);
        
    }

    private void WanaQuestionDisable()
    {
        animator.SetBool("wana", false);
    }

    private void ShowButtons()
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    private void SayYes()
    {
        audioSource.PlayOneShot(yesClip);
        animator.SetTrigger("yes");

        secretBackSource.Play();
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    private void SayNo()
    {
        audioSource.PlayOneShot(noClip);
        animator.SetTrigger("no");

        secretBackSource.Play();
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    private void DisableSecretFinish()
    {
        //FinishChouse.finishAnimationState = FinishChouse.FinishAnimationState.SET_SCORE;
        FinishChouse.finishAnimationState = FinishChouse.FinishAnimationState.STOP_PLAY;
        secretFinish.SetActive(false);
    }
}
