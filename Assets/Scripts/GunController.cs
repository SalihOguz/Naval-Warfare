using System.Collections;
using System.Collections.Generic;
using MirzaBeig.ParticleSystems.Demos;
using UnityEngine;

public class GunController : MonoBehaviour {
    [SerializeField]
    private GameObject _goRightSide;
    [SerializeField]
    private GameObject _goLeftSide;
    private Canon[] RightSide;
    private Canon[] LeftSide;
    private Canon[] AllCanons;
    public float CanonAngleChangeSpeed = 0.01f;
    public int MaxCanonAngle = 10;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private SoundController _soundController;

    void Start()
    {
        RightSide = _goRightSide.GetComponentsInChildren<Canon>();
        LeftSide = _goLeftSide.GetComponentsInChildren<Canon>();
        AllCanons = new Canon[RightSide.Length + RightSide.Length];
        RightSide.CopyTo(AllCanons, 0);
        LeftSide.CopyTo(AllCanons, RightSide.Length);
    }

    public void FireCanons(ShipSide side, bool isPlayer)
    {
        _audioSource.clip = _soundController.GetRandomSound();
        _audioSource.Play();

        StartCoroutine(StartFire(side, isPlayer));
    }

    private IEnumerator StartFire(ShipSide side, bool isPlayer)
    {
        yield return new WaitForSeconds(0.3f);
        int count = 0;
        foreach (Canon canon in (side == ShipSide.Right) ? RightSide : LeftSide)
        {
            StartCoroutine(FireDelay(canon, count / 5f));
            count++;
        }
    }

    private IEnumerator FireDelay(Canon canon, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        canon.FireCanon();

        //screen shake
        CameraShake cameraShake = FindObjectOfType<CameraShake>();
        cameraShake.Add(0.2f, 5.0f, 0.2f, CameraShakeTarget.Position, CameraShakeAmplitudeCurve.FadeInOut25);
        cameraShake.Add(4.0f, 5.0f, 0.5f, CameraShakeTarget.Rotation, CameraShakeAmplitudeCurve.FadeInOut25);      
    }

    public void ToggleTrajectories(ShipSide side, bool isActive)
    {
        foreach (Canon canon in (side == ShipSide.Right) ? RightSide : LeftSide)
        {
            canon.ToggleTrajectory(isActive);
        }
    }

    public void ToggleAllTrajectories(bool isActive)
    {
        foreach (Canon canon in AllCanons)
        {
            canon.ToggleTrajectory(isActive);
        }
    }

    public bool IsSideActive(ShipSide side)
    {
        return (side == ShipSide.Right) ? RightSide[0].IsTrajectoryActive() : LeftSide[0].IsTrajectoryActive();
    }

    public float GetCanonAngle()
    {
        return RightSide[0].transform.localEulerAngles.x;
    }

    public void SetCanonAngle(float diff)
    {
        float angle  = Mathf.Clamp(GetCanonAngle() - (diff * CanonAngleChangeSpeed), 359f - MaxCanonAngle, 359f);
        foreach (Canon canon in AllCanons)
        {
            Vector3 vec = canon.transform.localEulerAngles;
            canon.transform.localEulerAngles = Vector3.Lerp(canon.transform.localEulerAngles, new Vector3(angle, vec.y, vec.z), Time.deltaTime * 20f);
        }
    }

    public bool IsReadyToFire(ShipSide side)
    {
        return (side == ShipSide.Right) ? RightSide[0].IsReadyToFire : LeftSide[0].IsReadyToFire;
    }
}

public enum ShipSide
{
    Right,
    Left
}