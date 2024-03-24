using UnityEngine;

public class TakeItem : MonoBehaviour
{
    [SerializeField] private int _healPoints = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            playerHealth.Heal(_healPoints);
            playerHealth.CheckHealth();
            gameObject.SetActive(false);
        }
    }
}
