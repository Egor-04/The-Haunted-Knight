using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Init;
    [SerializeField] private float _timeSpawnPotion = 10f;
    [SerializeField] private GameObject _potion;
    [SerializeField] private GameObject _panelMenu;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _currTimeSpawnPotion = 10f;

    private void Awake()
    {
        Init = this;
        Time.timeScale = 1f;
        EventManager.onPlayerDead += ReloadScene;
    }

    private void Update()
    {
        _currTimeSpawnPotion -= Time.deltaTime;

        if (_currTimeSpawnPotion <= 0f)
        {
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                Instantiate(_potion, _spawnPoints[i].position, Quaternion.identity);
            }

            _currTimeSpawnPotion = _timeSpawnPotion;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            _panelMenu.SetActive(!_panelMenu.activeSelf);

            if (_panelMenu.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void PauseOnOff()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
