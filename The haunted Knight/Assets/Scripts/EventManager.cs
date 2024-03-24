using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action onPlayerDead;
    public static Action onEnemyDead;
    public static Action<GameObject> onEnemyDelete;
}
