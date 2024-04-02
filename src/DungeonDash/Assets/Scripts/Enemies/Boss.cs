using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject projectilePrefab;
    private BoxCollider2D boxCollider;

    public float fireInterval = 0.75f;
    private float fireTimer = 0f;

    private float angleOffset = 22.5f;

    public void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
        }

        CheckForPlayerCollision();
    }

    public void Fire()
    {
        int numberOfProjectiles = 16;
        float angle = 360f / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, i * angle + angleOffset);
            ShootProjectile(rotation);
        }
        angleOffset += 3f;
    }

    private void ShootProjectile(Quaternion direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().parent = gameObject.name;

        projectile.transform.rotation = direction;

        Vector3 shootDirection = projectile.transform.right;
        projectile.GetComponent<Rigidbody2D>().velocity = shootDirection * projectile.GetComponent<Projectile>().speed;
    }

    private void CheckForPlayerCollision()
    {
        Collider2D[] overlappingColliders = new Collider2D[5];
        int numOverlaps = boxCollider.OverlapCollider(new ContactFilter2D(), overlappingColliders);

        for (int i = 0; i < numOverlaps; i++)
        {
            Collider2D collider = overlappingColliders[i];
            if (collider.CompareTag("Player"))
            {
                CharacterHealth playerHealth = collider.gameObject.GetComponent<CharacterHealth>();
                playerHealth.TakeDamage(1, gameObject.name);
            }
        }
    }
}
