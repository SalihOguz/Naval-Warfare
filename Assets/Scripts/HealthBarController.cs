using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private DamageController _damageController;
    private Transform _mainCamera;
    
    [SerializeField]
    private Image _barFG;

    private float fullHealth;

    private void Start() {
        if (_damageController.ShipController.IsPlayer)
        {
            gameObject.SetActive(false);
            enabled = false;
        }

        fullHealth = _damageController.Health;
        _mainCamera = Camera.main.transform;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update() {
        transform.LookAt(_mainCamera);
        _barFG.fillAmount = _damageController.Health / fullHealth;
    }
}
