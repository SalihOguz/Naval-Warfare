using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private Text _txtVictoryTimer;

    [SerializeField]
    private GameObject _winPopup;

    [SerializeField]
    private GameObject _losePopup;

    private float fullHealth;
    
    public int TotalTime = 600;

    private void Start() {
        fullHealth = _damageController.Health;

        StartCoroutine(Refresh());
        EnemyController.Instance.EnemyShipSinkedEvent += RefreshKillCount;
        EnemyController.Instance.PlayerDeadEvent += Lose;

        StartCoroutine(StartDelay());
        StartCoroutine(RefreshTimer());
    } 

    IEnumerator StartDelay()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0;
    }

    private void OnDisable() {
        EnemyController.Instance.EnemyShipSinkedEvent -= RefreshKillCount;
    }

    private IEnumerator RefreshTimer()
    {
        _txtVictoryTimer.text = "Time Left: " + TotalTime/60 + ":" + TotalTime%60;
        TotalTime -= 1;
        if (TotalTime <= 0)
        {
            Lose();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(RefreshTimer());
        }
    }

    private IEnumerator Refresh()
    {
        yield return new WaitForSeconds(0.1f);

        _imgHealthBarFg.fillAmount = _damageController.Health / fullHealth;
        
        _txtLeftCanonCount.text = _gunController.IsReadyToFire(ShipSide.Left) ? "6" : "0";
        _txtRightCanonCount.text = _gunController.IsReadyToFire(ShipSide.Right) ? "6" : "0";
        StartCoroutine(Refresh());
    }

    private void RefreshKillCount()
    {
        _txtKillCount.text = "Kills: " + EnemyController.Instance.SinkedCount + "/8";

        if (EnemyController.Instance.SinkedCount == 8)
        {
            _winPopup.SetActive(true);
        }
    }

    private void Lose()
    {
        _losePopup.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
}
