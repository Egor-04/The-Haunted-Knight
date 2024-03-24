using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money InitMoney;
    [SerializeField] private int _moneyCount;

    private void Awake()
    {
        InitMoney = this;
    }

    public void RewardCoins(int rewardCoins)
    {
        _moneyCount += rewardCoins;
    }
}
