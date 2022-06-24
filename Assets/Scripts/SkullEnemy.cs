using System.Collections;
using UnityEngine;

public class SkullEnemy : MoveController
{
    public GameObject skullRotation;
    public AudioSource audioSource;
    public AudioClip skullCreate, skullDamage;

    private float speedY = -6;
    private float rotateSpeed;
    private int futurePosition;

    private void Start()
    {
        numPositions = 5;
        movementUnit = 2.5f / 2f;
        InitMoveController();

        SetRandomParameters();
        StartCoroutine(DestroyIfOutOfBonds());
        FindAndSetFutureRoad();
        audioSource.clip = skullCreate;
        audioSource.Play();
    }

    private void FindAndSetFutureRoad()
    {
        float[] variations = new float[] { 6, 6, 23, 32, 32 };
        if (currentLine > numPositions / 2)
        {
            float[] varReverse = new float[] { variations[4], variations[3], variations[2], variations[1], variations[0] };
            variations = varReverse;
        }
        float summOfVariations = variations[0] + variations[1] + variations[2] + variations[3] + variations[4];
        float probability = Random.Range(0, summOfVariations);
        if (probability < variations[0])
        {
            futurePosition = 0;
        }
        else if (probability < (variations[0] + variations[1]))
        {
            futurePosition = 1;
        }
        else if (probability < (variations[0] + variations[1] + variations[2]))
        {
            futurePosition = 2;
        }
        else if (probability < (variations[0] + variations[1] + variations[2] + variations[3]))
        {
            futurePosition = 3;
        }
        else
        {
            futurePosition = 4;
        }

        MoveToRoad(futurePosition);
    }

    private void SetRandomParameters()
    {
        int dirRotation = Random.Range(-1, 1);
        if (dirRotation == 0)
        {
            dirRotation = 1;
        }
        rotateSpeed = Random.Range(10f, 20f);
        rotateSpeed *= (float)dirRotation;
        speedY = Random.Range(-5f, -8f);
    }

    IEnumerator DestroyIfOutOfBonds()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (transform.position.y < -6f)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        skullRotation.transform.Rotate(new Vector3(0, 0, rotateSpeed), Space.World);
        transform.Translate(new Vector2(0, -speedY) * Time.deltaTime);
        if (Mathf.Abs(transform.position.x - futurePosition) < 0.04f)
        {
            speedX = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            return;
        }
        else if (other.tag.Equals("Bulb"))
        {
            ScoreManager.IncreaseScore(ScoreVolumes.enemyDestroy);
            Destroy(gameObject);
        }
        else if (other.tag.Equals("Player"))
        {
            audioSource.PlayOneShot(skullDamage);
            ScoreManager.DecreaseScore(ScoreVolumes.enemyHeat);
        }
    }
}