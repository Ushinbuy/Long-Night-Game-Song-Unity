using UnityEngine;

public class MoveDown : MonoBehaviour
{
    [SerializeField] private float speedY;
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speedY, Space.World);
    }
}
