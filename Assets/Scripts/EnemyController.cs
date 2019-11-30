using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;
    public UnityAction EnemyShipSinkedEvent;

    public UnityAction PlayerDeadEvent;
    public int SinkedCount;

    private void Awake() {
        Instance = this;
    }

    public void EnemyShipDead()
    {
        SinkedCount ++;
        EnemyShipSinkedEvent?.Invoke();
    }

    public void PlayerDead()
    {
        PlayerDeadEvent?.Invoke();
    }

}
