using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform marsTransform;
    private float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform mars, float moveSpeed)
    {
        marsTransform = mars;
        speed = moveSpeed;
    }

    void FixedUpdate()
    {
        if (marsTransform == null) return;

        // Richting naar Mars
        Vector2 directionToMars = (marsTransform.position - transform.position).normalized;

        // Velocity instellen zodat de enemy naar Mars beweegt
        rb.velocity = directionToMars * speed;

        // Rotatie naar Mars
        float angle = Mathf.Atan2(directionToMars.y, directionToMars.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90;
    }
}
