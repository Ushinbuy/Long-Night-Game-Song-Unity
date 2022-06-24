using System.Collections;
using UnityEngine;

public abstract class Enemy : MoveController
{
    [SerializeField] protected float speedY;
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip destroy;

    protected readonly float lowerBound = -7.0f;
    private EnemyState enemyState;

    private enum EnemyState
    {
        NONE,
        DESTROYED
    }

    protected virtual void Start()
    {
        InitMoveController();
        enemyState = EnemyState.NONE;
        StartCoroutine(DestroyIfOutOfBonds());
    }

    void Update()
    {
        MoveEnemy();
        if(enemyState == EnemyState.DESTROYED)
        {
            audioSource.volume -= 0.08f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            return;
        }
        else if (other.tag.Equals("Battery"))
        {
            return;
        }

        if (other.tag.Equals("Player"))
        {
            ScoreManager.DecreaseScore(ScoreVolumes.enemyHeat);
        }
        else if (other.tag.Equals("Bulb"))
        {
            audioSource.PlayOneShot(destroy, 1.56f);
            ScoreManager.IncreaseScore(ScoreVolumes.enemyDestroy);
            enemyState = EnemyState.DESTROYED;
        }
        else
        {

        }
        StopAllCoroutines();
        animator.SetTrigger("death");
    }

    private void EnemyDestroy()
    {
        Destroy(gameObject);
    }
    protected void MoveEnemy()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speedY);
    }
    protected IEnumerator DestroyIfOutOfBonds()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (transform.position.y < lowerBound)
            {
                Destroy(gameObject);
                //ScoreManager.wasDamagedOneTime = true;
                ScoreManager.DecreaseScore(ScoreVolumes.enemyOutOfDisplay);
            }
        }
    }
}