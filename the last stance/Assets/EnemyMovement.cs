using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;

    public void Initialize(Vector3 direction, float speed)
    {
        moveDirection = direction;
        moveSpeed = speed;
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
