using System.Collections;
using UnityEngine;

public class OctoppusEnemy : Enemy
{
    [SerializeField] private AudioClip atackSound;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SetAtackTrigger());
    }

    IEnumerator SetAtackTrigger()
    {
        while (true)
        {
            float time = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(time);
            StartCoroutine(PauseShotAudio());
            animator.SetTrigger("atack");
        }
    }

    IEnumerator PauseShotAudio()
    {
        yield return new WaitForSeconds(0.6f);
        audioSource.PlayOneShot(atackSound, 0.43f);
    }
}
