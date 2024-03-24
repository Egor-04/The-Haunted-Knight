using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SwordWeapon : MonoBehaviour
{
    [SerializeField] private int _minDamage = 4, _maxDamage = 10;
    [SerializeField] private float _discardingForce = 5f;
    [SerializeField] private float _stunTime = 3f;
    [SerializeField] private float _interval = 0.8f;
    [SerializeField] private Image _hitImage;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _soundSwordWave;
    [SerializeField] private AudioClip _soundSwordHit;
    [SerializeField] private bool _splashDamageActive;

    [Header("Damage Box")]
    [SerializeField] private float _angle;
    [SerializeField] private Transform _damageBox;
    [SerializeField] private Vector2 _boxSize;
    
    [Header("Damage Hit Image")]
    [SerializeField] private float _currentInterval;
    [SerializeField] private Collider2D _enemy;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private WaveController _waveController;

    private void Awake()
    {
        //_waveController = FindObjectOfType<WaveController>();
        EventManager.onEnemyDead += ClearEnemyGo;
        _waveController = FindObjectOfType<WaveController>();
    }

    private void Update()
    {
        _currentInterval -= Time.deltaTime;

        Attack();
    }

    private void Attack()
    {
        _enemy = Physics2D.OverlapBox(_damageBox.position, _boxSize, _angle, _enemyMask);

        if (_currentInterval <= 0f)
        {
            if (_hitImage.fillAmount >= 0.9f)
            {
                _hitImage.fillAmount = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_enemy)
                {
                    if (_enemy.CompareTag("Enemy"))
                    {

                        if (_splashDamageActive == false)
                        {
                            RegularDamage();
                        }
                        else
                        {
                            SplashDamage();
                        }
                        
                        if (_enemy)
                        {
                            if (DistanceToEnemy(_enemy.gameObject) > 8f)
                            {
                                _enemy = null;
                            }
                        }
                    }
                }

                _source.PlayOneShot(_soundSwordWave);
                _currentInterval = _interval;
                StartCoroutine(HitImage());
            }
        }
    }

    private void RegularDamage()
    {
        Vector2 directionDiscarding = _enemy.transform.position - transform.position;
        Health enemyHealth = _enemy.GetComponent<Health>();
        Collider2D enemyCollider = _enemy.GetComponent<Collider2D>(); 

        if (enemyHealth.GetHealthInfo() > 0f)
        {
            StunDiscarding(enemyCollider, directionDiscarding);
            enemyHealth.Damage(_minDamage, _maxDamage);

            if (_source.isPlaying == false)
            {
                _source.PlayOneShot(_soundSwordHit);
            }
        }
    }

    private void SplashDamage()
    {
        for (int i = 0; i < _waveController.AllEnemies.Count; i++)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_damageBox.position, _boxSize, _angle);

            for (int j = 0; j < colliders.Length; j++)
            {
                Vector2 directionDiscarding = colliders[j].transform.position - transform.position;
                
                if (_waveController.AllEnemies[i] == colliders[j].gameObject)
                {
                    StunDiscarding(colliders[j], directionDiscarding);
                    Health enemyHealth = colliders[j].GetComponent<Health>();
                    enemyHealth.Damage(_minDamage, _maxDamage);
                }
            }

            if (_source.isPlaying == false)
            {
                _source.PlayOneShot(_soundSwordHit);
            }
        }
    }

    private void StunDiscarding(Collider2D collider, Vector2 directionDiscarding)
    {
        Enemy enemyScript = collider.GetComponent<Enemy>();
        enemyScript.StunEnemy(_stunTime);

        Rigidbody2D enemyRb2 = collider.GetComponent<Rigidbody2D>();
        enemyRb2.AddForce(directionDiscarding * _discardingForce);
    }

    private void ClearEnemyGo()
    {
        _enemy = null;
    }

    public float DistanceToEnemy(GameObject go)
    {
        float distance = (transform.position - go.transform.position).sqrMagnitude;
        return distance;
    }

    private IEnumerator HitImage()
    {
        while (_hitImage.fillAmount < 0.9f)
        {
            _hitImage.fillAmount = Mathf.Lerp(_hitImage.fillAmount, 1f, 10f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_damageBox.position, _boxSize);
    }
}
