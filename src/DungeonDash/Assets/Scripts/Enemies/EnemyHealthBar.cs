using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public float initialHealthBarWidth = 1f;

    private void Start()
    {
        transform.localScale = new Vector3(initialHealthBarWidth, transform.localScale.y, transform.localScale.z);
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        if (healthPercentage < 0) healthPercentage = 0;
        float newHealthBarWidth = initialHealthBarWidth * healthPercentage;
        transform.localScale = new Vector3(newHealthBarWidth, transform.localScale.y, transform.localScale.z);
    }
}
