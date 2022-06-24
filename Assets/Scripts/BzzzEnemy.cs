using System.Collections;
using UnityEngine;

public class BzzzEnemy : Enemy
{
    [SerializeField] private AudioClip atackSound;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SetCrossRoadTrigger());
    }

    IEnumerator SetCrossRoadTrigger()
    {
        while (true)
        {
            float time = Random.Range(0.8f, 4.0f);
            yield return new WaitForSeconds(time);
            animator.SetTrigger("cross_road");
            audioSource.PlayOneShot(atackSound, 0.5f);
            if (currentLine == 0)
            {
                SetRightMove();
            }
            else if (currentLine == numPositions - 1)
            {
                SetLeftMove();
            }
            else
            {
                int randForRoad = Random.Range(0, numPositions);
                if (randForRoad == 1)
                {
                    SetLeftMove();
                }
                else
                {
                    SetRightMove();
                }
            }
        }
    }
}