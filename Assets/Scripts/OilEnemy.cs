using System.Collections;
using UnityEngine;

public class OilEnemy : Enemy
{
    [SerializeField] private GameObject oilShot;
    [SerializeField] private AudioClip atackSound;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SetShotTrigger());
    }

    IEnumerator SetShotTrigger()
    {
        while (true)
        {
            float time = Random.Range(0.8f, 2.0f);
            yield return new WaitForSeconds(time);
            animator.SetTrigger("shot");
            StartCoroutine(PauseShotAudio());
        }
    }

    IEnumerator PauseShotAudio()
    {
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(atackSound, 0.8f);
    }

    void CreateBullet()
    {
        //this function called by animation "shot"
        Instantiate(oilShot, transform.position, transform.rotation);
    }
}
