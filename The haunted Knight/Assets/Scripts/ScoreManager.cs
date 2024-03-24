using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int _currentKillCount;
    [SerializeField] private int _bestKillCount;
    [SerializeField] private TMP_Text _bestCountText;
    [SerializeField] private TMP_Text _currentKillCountText;

    private void Awake()
    {
        LoadBestCountKills();
        EventManager.onEnemyDead = AddKill;
        EventManager.onPlayerDead = SaveBestCountKills;
    }

    private void AddKill()
    {
        _currentKillCount++;
        _currentKillCountText.text = "Убийства: " + _currentKillCount;
    }

    private void SaveBestCountKills()
    {
        if (PlayerPrefs.HasKey("KILLS"))
        {
            if (PlayerPrefs.GetInt("KILLS") < _currentKillCount)
            {
                PlayerPrefs.SetInt("KILLS", _currentKillCount);
            }
        }
        else
        {
            PlayerPrefs.SetInt("KILLS", _currentKillCount);
        }
    }

    private void LoadBestCountKills()
    {
        if (PlayerPrefs.HasKey("KILLS"))
        {
            _bestCountText.text = "Рекорд: " + PlayerPrefs.GetInt("KILLS").ToString();
        }
    }
}
