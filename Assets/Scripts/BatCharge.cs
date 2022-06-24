using System.Collections;
using UnityEngine;

public class BatCharge : MonoBehaviour
{
    [SerializeField] protected float speedY;
    [SerializeField] private BoxCollider boxCollider;

    private bool playerGetBattery;
    private Vector2 speedFinish;

    private readonly float lowerBound = -7.0f;
    private readonly float xFinishCoord = 2.47f;
    private readonly float yFinishCoord = -4.4f;
    private readonly float timeForFinishi = 0.4f;

    void Start()
    {
        StartCoroutine(DestroyIfOutOfBonds());
        playerGetBattery = false;
    }

    void Update()
    {
        if(!playerGetBattery)
        {
            transform.Translate(Vector2.down * Time.deltaTime * speedY, Space.World);
        }
        else
        {
            transform.Translate(speedFinish * Time.deltaTime, Space.World);
            if(transform.position.x > xFinishCoord)
            {
                BatteryScore.IncreaseScore();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator DestroyIfOutOfBonds()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (transform.position.y < lowerBound)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(BatteryScore.Score == 4)
            {
                return;
            }
            playerGetBattery = true;
            boxCollider.enabled = false;
            speedFinish = new Vector2(xFinishCoord - transform.position.x, yFinishCoord - transform.position.y) / timeForFinishi;
            //Destroy(other.gameObject);
        }
        else if (other.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
