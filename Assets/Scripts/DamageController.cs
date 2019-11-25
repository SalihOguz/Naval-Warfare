using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
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

        _shipRigidBody.mass = 100f;
        // TODO particle effect
    }
}
