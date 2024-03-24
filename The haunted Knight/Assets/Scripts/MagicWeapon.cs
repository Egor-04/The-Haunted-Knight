using System.Collections;
using UnityEngine;

public class MagicWeapon : MonoBehaviour
{
    [SerializeField] private int _magicSphereCount = 5;
    [SerializeField] private float _timeToCollectingEnergyShell = 3f;
    [SerializeField] private float _magicEnergySpeed = 20f;
    [SerializeField] private float _magicSphereSpreadAngle = 30f;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private GameObject _magicEnergy;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _magicCollectingSound;
    [SerializeField] private AudioClip _magicEnergyShotSound;
    [SerializeField] private ParticleSystem _magicEffect;
    [SerializeField] private bool _shotReady = false;
    [SerializeField] private float _currentTimeToShot;
    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        FirePointToMousePosition();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _currentTimeToShot += Time.deltaTime;

            if (_currentTimeToShot > _timeToCollectingEnergyShell)
            {
                _shotReady = true;
            }
        }
        else
        {
            if (_currentTimeToShot >= _timeToCollectingEnergyShell)
            {
                _currentTimeToShot = 0f;
                
                for (int i = 0; i < _magicSphereCount; i++)
                {
                    Fire();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _magicEffect.Play();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _magicEffect.Stop();
        }
    }

    private void Fire()
    {
        float bulletSpreadAngle = Random.Range(-_magicSphereSpreadAngle, _magicSphereSpreadAngle);

        GameObject bullet = Instantiate(_magicEnergy, _firePoint.position, _firePoint.rotation);
        bullet.transform.position = new Vector3(bullet.transform.position.x, bullet.transform.position.y, 0f);
        bullet.transform.Rotate(0f, 0f, bulletSpreadAngle);

        Vector2 direction = _firePoint.position - _player.transform.position;
        Vector2 randomDirection = Quaternion.AngleAxis(Random.Range(-_magicSphereSpreadAngle, _magicSphereSpreadAngle), Vector3.forward) * direction; // добавляем случайный разброс в определенном диапазоне углов

        Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
        rb2d.AddForce(randomDirection * _magicEnergySpeed, ForceMode2D.Impulse);
        //_source.PlayOneShot(_magicEnergyShotSound);
        _shotReady = false;
    }

    private void FirePointToMousePosition()
    {
        _firePoint.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = (_player.position - _firePoint.position).sqrMagnitude;

        if (distance > _radius)
        {
            Vector3 dir = (_player.position - _firePoint.position).normalized; // вычисляем направление движения объекта к центру ограниченной области
            Vector3 newPos = _player.position - dir * _radius; // вычисляем новые координаты для объекта, не допуская выхода за границы
            _firePoint.position = newPos; // устанавливаем новые координаты для объекта
        }
        //_firePoint.position = new Vector3(Mathf.Clamp(_firePoint.position.x, _player.position.x - 1.2f, _player.position.x + 1.2f), Mathf.Clamp(_firePoint.position.y, _player.position.y - 1.2f, _player.position.y + 1.2f), 0f);
    }
}