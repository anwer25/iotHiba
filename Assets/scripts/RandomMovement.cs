using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float speed = 1f; // The speed at which the object moves
    private Rigidbody _rb; // The Rigidbody component on the object
    private Vector3 _direction; // The direction in which the object is moving
    private bool _canMove;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // Set a random initial direction for the object
        _direction = GetRandomDirection();
    }

    private void FixedUpdate()
    {
        // Set the velocity of the Rigidbody component to move the object in the XZ plane only
        var xzVelocity = new Vector3(_direction.x, 0f, _direction.z).normalized * speed;
        _rb.velocity = xzVelocity;


        if (Physics.Raycast(transform.position, _direction, out var hit, 4f))
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            if (hit.collider.gameObject.GetComponent<BoxCollider>() != null)
            {
                // Change direction by picking a new random direction
                _direction = GetRandomDirection();
            }
        }

        if (!(transform.position.y < 2)) return;
        var transform1 = transform;
        var position = transform1.position;
        position = new Vector3(position.x, 2, position.z);
        transform1.position = position;
    }
    
    private static Vector3 GetRandomDirection()
    {
        // Get a random direction on the XZ plane
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }
}