using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float _speed, _cameraSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private GameObject _swords;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _steps;
    private int _numSound;

    private void Start()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        StepsSound();
        LookToSide();
    }

    private void FixedUpdate()
    {
        Movement();
        CameraFollow();
    }

    private void CameraFollow()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, transform.position, _cameraSpeed * Time.deltaTime);
        _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, -10);
    }

    private void LookToSide()
    {
        Vector2 mousePosition = Input.mousePosition;

        if (mousePosition.x < Screen.width / 2)
        {
            _playerSprite.flipX = true;
            _swords.transform.eulerAngles = new Vector3(_swords.transform.eulerAngles.x, -180f, _swords.transform.eulerAngles.z);
        }
        else
        {
            _playerSprite.flipX = false;
            _swords.transform.eulerAngles = new Vector3(_swords.transform.eulerAngles.x, 0f, _swords.transform.eulerAngles.z);
        }
    }

    private void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(horizontal, vertical);

        Animation(vertical, horizontal);
        _rb.MovePosition(_rb.position + direction * _speed * Time.fixedDeltaTime);
    }

    private void StepsSound()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            for (int i = 0; i < _steps.Length; i++)
            {
                if (_source.isPlaying == false)
                {
                    if (_numSound <= _steps.Length-1)
                    {
                        _source.clip = _steps[_numSound];
                        _source.Play();
                        _numSound++;
                    }
                    else
                    {
                        _numSound = 0;
                    }
                }
            }
        }
    }

    private void Animation(float vertical, float horizontal)
    {
        if (_animator)
        {
            if (vertical != 0 || horizontal != 0)
            {
                _animator.SetBool("Walk", true);
            }
            else
            {
                _animator.SetBool("Walk", false);
            }
        }
    }
}
