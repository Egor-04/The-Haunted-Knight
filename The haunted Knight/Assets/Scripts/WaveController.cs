using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private int _waveCount = 1;
    [SerializeField] private int _enemyCount;
    [SerializeField] private int _coefficentCount = 1;
    [SerializeField] private TMP_Text _textWave;
    [SerializeField] private Animator _waveAnimator;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _enemyPrefab;
    public List<GameObject> AllEnemies;
    private int _spawnPointIndex;

    private void Awake()
    {
        AllEnemies = new List<GameObject>();
        EventManager.onEnemyDelete += DeleteEnemy;
    }

    private void Update()
    {
        CreateNewWave();
    }

    private void CreateNewWave()
    {
        if (AllEnemies.Count <= 0)
        {
            _textWave.enabled = true;
            _textWave.text = "Волна " + _waveCount.ToString();
            _waveAnimator.SetTrigger("Show");


            for (int i = 0; i < _enemyCount; i++)
            {
                for (int j = 0; j < _spawnPoints.Length; j++)
                {
                    _spawnPointIndex = j;
                }

                GameObject enemy = Instantiate(_enemyPrefab, _spawnPoints[_spawnPointIndex].position, Quaternion.identity);
                AllEnemies.Add(enemy);
            }
            _enemyCount += _coefficentCount;
            _waveCount++;
        }
    }

    private void DeleteEnemy(GameObject enemy)
    {
        AllEnemies.Remove(enemy);
    }
}
