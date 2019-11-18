using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canonball : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _gravityEffect;
    [HideInInspector]
    public Vector3 GravityEffect
    {
        get=> _gravityEffect;
        set=> _gravityEffect = value;
    }
    private float _horizontalAngle;
    private float _elevationAngle;
    private float _initialSpeed;
    [HideInInspector]
    public float InitialSpeed
    {
        get=> _initialSpeed;
        set=> _initialSpeed = value;
    }
    private bool _canonLaunched;

    void Update()
    {
        if (_canonLaunched)
        {
            float dt = Time.deltaTime;
            _velocity += _gravityEffect * dt;
            transform.position += _velocity * dt;
        }
    }

    Vector3 ComputeInitialVelocity()
    {
        float sinTheta = Mathf.Sin(_horizontalAngle);
        float cosTheta = Mathf.Cos(_horizontalAngle);
        float sinPhi = Mathf.Sin(_elevationAngle);
        float cosPhi = Mathf.Cos(_elevationAngle);
        
        return _initialSpeed * new Vector3(cosPhi * sinTheta, -sinPhi, cosPhi * cosTheta);
    }

    public void FireCannon(float horizontalAngle, float elevationAngle)
    {
        _horizontalAngle = horizontalAngle;
        _elevationAngle = elevationAngle;

        _velocity = ComputeInitialVelocity();
        _canonLaunched = true;
    }
}
