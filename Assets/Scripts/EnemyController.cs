using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;
    public UnityAction EnemyShipSinked;
    public int SinkedCount;

    private void Awake() {
        Instance = this;
    }

    public void EnemyShipDead()
    {
        EnemyShipSinked?.Invoke();
        SinkedCount ++;
    }

    public void PlayerDead()
    {
        // TODO show a UI and restart button
    }

}
