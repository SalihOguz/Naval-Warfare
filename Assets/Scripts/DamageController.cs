using System.Collections;
using System.Collections.Generic;
using MirzaBeig.ParticleSystems.Demos;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public ShipController ShipController;
    public float Health = 150f;
    public float CanonDamage = 10f;

    [SerializeField]
    private GameObject _nonDamagedObjects;

    [SerializeField]
    private GameObject _damagedObjects;

    [SerializeField]
    private Mesh _damagedHull;

    [SerializeField]
    private MeshFilter _hullMeshFilter;

    [SerializeField]
    private Rigidbody _shipRigidBody;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Canonball")
        {
            Health -= CanonDamage;

            if (ShipController.IsPlayer)
            {
                CameraShake cameraShake = FindObjectOfType<CameraShake>();
                cameraShake.Add(1f, 2.0f, 0.2f, CameraShakeTarget.Position, CameraShakeAmplitudeCurve.FadeInOut25);
                cameraShake.Add(6.0f, 2.0f, 0.3f, CameraShakeTarget.Rotation, CameraShakeAmplitudeCurve.FadeInOut25);  
            }

            if (Health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        _hullMeshFilter.mesh = _damagedHull;
        _damagedObjects.SetActive(true);
        _nonDamagedObjects.SetActive(false);
        GetComponent<MeshCollider>().enabled = false;

        _shipRigidBody.mass = 100f;
        // TODO particle effect

        if (!ShipController.IsPlayer)
        {
            EnemyController.Instance.EnemyShipDead();
        }   
        else
        {
            EnemyController.Instance.PlayerDead();
        }
    }
}
