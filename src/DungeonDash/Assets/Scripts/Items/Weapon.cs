using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public float radius = 0.45f; // Radius of rotation
    public float yOffset = 0.4f; // Offset the center of rotation down
    public GameObject arrowPrefab;

    public string parent;

    public float weaponRotation = 45f;

    private bool canAttack = true;
    private float attackCooldown = 1f;
    public int weaponDamage = 10;

    void Update()
    {
        RotateAroundPlayer();
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnFire(InputValue inputValue)
    {
        if (canAttack)
        {
            // Get the direction from the player to the cursor
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = cursorPosition - transform.parent.position;

            // Calculate the angle in radians
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject projectile = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
            projectile.GetComponent<Projectile>().parent = parent;
            projectile.GetComponent<Projectile>().damage = weaponDamage;

            canAttack = false;

            // Start fire cooldown
            StartCoroutine(AttackCooldown());
        }
    }

    void RotateAroundPlayer()
    {
        if (transform.parent != null)
        {
            // Get the direction from the player to the cursor
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = cursorPosition - transform.parent.position;

            // Calculate the angle in radians
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Calculate the weapon's position relative to the player
            Vector3 weaponPosition = transform.parent.position + Quaternion.Euler(0f, 0f, angle) * (Vector3.right * radius);
            weaponPosition.y -= yOffset;

            transform.position = weaponPosition;

            // Rotate the weapon towards the cursor
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Apply a flat 45 degree rotation to the bow itself, since the icon sprites are angled 45 degrees.
            transform.localRotation *= Quaternion.Euler(0f, 0, weaponRotation);
        }
    }

    public void EquipNewWeapon(int newWeaponDamage, Sprite newWeaponSprite) 
    { 
        weaponDamage = newWeaponDamage;
        GetComponent<SpriteRenderer>().sprite = newWeaponSprite;
    }
}