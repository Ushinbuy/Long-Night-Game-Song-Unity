using System.Collections;
using UnityEngine;

public class LittleObjectsMovement : MonoBehaviour
{
    private Vector2 startPosition;
    private float speedObjectX, speedObjectY;
    void Start()
    {
        startPosition = transform.position;
        int signX;
        if(startPosition.x > 0)
        {
            signX = -1;
        }
        else
        {
            signX = 1;
        }
        speedObjectY = -1f * Random.Range(0.5f, 1.7f);
        speedObjectX = signX * Random.Range(0.5f, 3f);
        float scale = Random.Range(0.5f, 1.2f);
        transform.localScale = new Vector3(scale * signX, scale, 1);
        StartCoroutine(DestroyIfOutOfBonds());
    }

    void Update()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        transform.Translate(Time.deltaTime * speedObjectX, Time.deltaTime * speedObjectY, 0);
    }

    IEnumerator DestroyIfOutOfBonds()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            float positionForDestroyX = 5.5f;
            if (transform.position.x < -positionForDestroyX ||
                transform.position.x > positionForDestroyX)
            {
                Destroy(gameObject);
            }
        }
    }
}
