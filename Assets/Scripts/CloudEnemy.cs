using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudEnemy : Enemy
{
    [SerializeField] private GameObject skull;
    [SerializeField] private SpriteRenderer cloudSpriteRenderer;

    private Vector3 startScale;
    private bool startScalling = true;

    private readonly float increaseSpeed = 0.1f;

    protected override void Start()
    {
        numPositions = 5;
        movementUnit = 2.5f / 2f;
        base.Start();

        startScale = transform.localScale;
        transform.localScale = new Vector3(0.001f, 0.001f, 1);

        int numberOfFuturePosition = Random.Range(0, numPositions + 1);
        if(numberOfFuturePosition == 2)
        {
            int additions = Random.Range(-1, 1);
            if(additions == 0)
            {
                additions++;
            }
            numberOfFuturePosition += additions;
        }
        MoveToRoad(numberOfFuturePosition);
        StartCoroutine(CreateSkull());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(CreateSkull());
        }
        if (startScalling)
        {
            Vector3 increaseUnit = new Vector3(increaseSpeed, increaseSpeed, 0);
            transform.localScale += increaseUnit;
            if (transform.localScale.x > startScale.x)
            {
                startScalling = false;
            }
        }
        else
        {
            MoveEnemy();
        }
    }

    IEnumerator CreateSkull()
    {
        int maxSkulls = 10;
        for (int i = 0; i < maxSkulls; i++)
        {
            Instantiate(skull, transform.position, skull.transform.rotation);
            yield return new WaitForSeconds(0.8f);
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
            StartCoroutine(HeatColorAnimate());
        }
        else if (other.tag.Equals("Player"))
        {
            ScoreManager.DecreaseScore(ScoreVolumes.enemyHeat);
        }
    }

    IEnumerator HeatColorAnimate()
    {
        cloudSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        cloudSpriteRenderer.color = Color.white;
    }
}