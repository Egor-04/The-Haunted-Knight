using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewarded : MonoBehaviour
{
    [SerializeField] private int _rewardCoins = 3;

    [System.Obsolete]
    public void ShowRewarded()
    {
        //Money.InitMoney.RewardCoins(_rewardCoins);
        Application.ExternalCall("ShowRewardedAd");
    }
}