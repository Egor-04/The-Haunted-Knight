using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int minDamage = 2, maxDamage = 10;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private float _currentAttackInterval;
    [SerializeField] private float _distanceAttack = 12f;
    [SerializeField] private float _distanceFollow = 13f;
    [SerializeField] private float _targetDistancePlayer;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _currentPatroolPoint;
    [SerializeField] private float _timeToChangePatroolPoint = 10f;
    [SerializeField] private float _currentChangeTime;

    [Header("Special Options")]
    [SerializeField] private float _stunResistance = 0f;

    [Header("Base Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb2;
    [SerializeField] private SpriteRenderer _enemySprite;
    [SerializeField] private Transform _allPatroolPoints;
    [SerializeField] private Transform[] _patroolPoints;
    private int _pointIndex;
    private float _currentStunTime;


    private void Awake()
    {
        _allPatroolPoints = GameObject.FindGameObjectWithTag("Patrool Points").transform;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb2 = GetComponent<Rigidbody2D>();

        for (int i = 0; i < _allPatroolPoints.childCount; i++)
        {
            _patroolPoints[i] = _allPatroolPoints.GetChild(i);
        }

        _currentPatroolPoint = _patroolPoints[Random.Range(0, _patroolPoints.Length)];
    }

    private void Update()
    {
        _currentAttackInterval -= Time.deltaTime;
        _currentChangeTime -= Time.deltaTime;
        _currentStunTime -= Time.deltaTime;
        LookAtTarget();
    }

    private void FixedUpdate()
    {
        if (_currentStunTime <= 0f - _stunResistance)
        {
            FollowPlayer();
        }
    }

    public void StunEnemy(float stunTime)
    {
        _currentStunTime = stunTime;
    }

    private void FollowPlayer()
    {
        _targetDistancePlayer = (_player.transform.position - transform.position).sqrMagnitude;

        if (_targetDistancePlayer <= _distanceFollow)
        {
            _animator.SetBool("Walk", true);

            Vector2 direction = _player.position - transform.position;
            _rb2.MovePosition(_rb2.position + direction.normalized * _speed * Time.fixedDeltaTime);
        }
        else
        {
            Patrool();
        }
        
        if (_targetDistancePlayer <= _distanceAttack)
        {
            Health playerHealth = _player.GetComponent<Health>();
            
            if (_currentAttackInterval <= 0f)
            {
                playerHealth.Damage(minDamage, maxDamage);
                _currentAttackInterval = _attackInterval;
            }
            _animator.SetBool("Walk", false);
        }
    }

    private void Patrool()
    {
        float currentPointDistance = (_currentPatroolPoint.position - transform.position).sqrMagnitude;

        if (_currentPatroolPoint)
        {
            for (int i = 0; i < _patroolPoints.Length; i++)
            {
                if (currentPointDistance <= 5f)
                {
                    ChangePatroolPoint();
                }
            }

            if (_currentChangeTime <= 0f)
            {
                ChangeRandomPatroolPoint();
                _currentChangeTime = _timeToChangePatroolPoint;
            }

            Vector2 targetDirection = _currentPatroolPoint.position - transform.position;
            _rb2.MovePosition(_rb2.position + targetDirection.normalized * _speed * Time.fixedDeltaTime);
        }
    }

    private void ChangePatroolPoint()
    {
        if (_pointIndex < _patroolPoints.Length)
        {
            _currentPatroolPoint = _patroolPoints[_pointIndex];
            _pointIndex++;
        }
        else if (_pointIndex == _patroolPoints.Length)
        {
            _pointIndex = 0;
            _currentPatroolPoint = _patroolPoints[_pointIndex];
        }
    }

    private void ChangeRandomPatroolPoint()
    {
        if (_pointIndex < _patroolPoints.Length)
        {
            _currentPatroolPoint = _patroolPoints[Random.Range(0, _patroolPoints.Length)];
            _pointIndex++;
        }
        else
        {
            _pointIndex = 0;
            _currentPatroolPoint = _patroolPoints[Random.Range(0, _patroolPoints.Length)];
        }
    }

    private void LookAtTarget()
    {
        if (_targetDistancePlayer <= _distanceFollow)
        {
            Vector2 directionPlayer = _player.transform.position - transform.position;
            if (directionPlayer.x > 0)
            {
                _enemySprite.flipX = false;
            }
            else
            {
                _enemySprite.flipX = true;
            }
        }
        else
        {
            Vector2 directionPoint = _currentPatroolPoint.position - transform.position;
            if (directionPoint.x > 0)
            {
                _enemySprite.flipX = false;
            }
            else
            {
                _enemySprite.flipX = true;
            }
        }
    }
}
