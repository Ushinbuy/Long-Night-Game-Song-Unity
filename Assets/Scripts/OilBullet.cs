using System.Collections;
using UnityEngine;

public class OilBullet : Enemy
{
    new IEnumerator DestroyIfOutOfBonds()
    {
        while (true)
        {
            if (transform.position.y < lowerBound)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            return;
        }
        else if (other.tag.Equals("Player"))
        {
            ScoreManager.DecreaseScore(ScoreVolumes.oilBulletHeat);
        }
        else if (other.tag.Equals("Bulb"))
        {
            ScoreManager.IncreaseScore(ScoreVolumes.oilBulletDestroy);
        }
        Destroy(gameObject);
    }
}
