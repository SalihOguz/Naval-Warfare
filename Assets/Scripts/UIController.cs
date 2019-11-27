using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private DamageController _damageController;

    [SerializeField]
    private GunController _gunController;

    [SerializeField]
    private Image _imgHealthBarFg;

    [SerializeField]
    private Text _txtLeftCanonCount;

    [SerializeField]
    private Text _txtRightCanonCount;

    [SerializeField]
    private Text _txtKillCount;

    private float fullHealth;

    private void Start() {
        fullHealth = _damageController.Health;

        StartCoroutine(Refresh());
        EnemyController.Instance.EnemyShipSinked += RefreshKillCount;
    } 

    private void OnDisable() {
        EnemyController.Instance.EnemyShipSinked -= RefreshKillCount;
    }

    private IEnumerator Refresh()
    {
        yield return new WaitForSeconds(0.1f);

        _imgHealthBarFg.fillAmount = _damageController.Health / fullHealth;
        
        _txtLeftCanonCount.text = _gunController.IsReadyToFire(ShipSide.Left) ? "6" : "0";
        _txtRightCanonCount.text = _gunController.IsReadyToFire(ShipSide.Right) ? "6" : "0";
    }

    private void RefreshKillCount()
    {
        _txtKillCount.text = "Kills: " + EnemyController.Instance.SinkedCount;
    }
}
