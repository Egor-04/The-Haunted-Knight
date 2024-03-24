using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private GameObject _damageText;

    public void Heal(int healPoints)
    {
        _health += healPoints;
    }

    public void Damage(int minForce, int maxForce)
    {
        int randomDamage = Random.Range(minForce, maxForce);
        _health -= randomDamage;
        GameObject dmgText = Instantiate(_damageText, transform.position, Quaternion.identity);
        dmgText.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "-" + randomDamage.ToString();
        CheckHealth();
    }

    public int GetHealthInfo()
    {
        return _health;
    }

    public void CheckHealth()
    {
        if (_health <= 0f)
        {
            if (gameObject.CompareTag("Player"))
            {
                EventManager.onPlayerDead!.Invoke();
            }

            if (gameObject.CompareTag("Enemy"))
            {
                EventManager.onEnemyDead?.Invoke();
                EventManager.onEnemyDelete?.Invoke(gameObject);
            }

            _healthSlider.value = _health;
            _healthText.text = _health.ToString() + "%";
            gameObject.SetActive(false);
            _health = 0;
        }

        if (_health >= 100)
        {
            _health = 100;
        }

        _healthText.text = _health.ToString() + "%";
        _healthSlider.value = _health;
    }
}
