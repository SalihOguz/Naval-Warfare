using System.Collections.Generic;
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

    void Start()
    {
        RightSide = _goRightSide.GetComponentsInChildren<Canon>();
        LeftSide = _goLeftSide.GetComponentsInChildren<Canon>();
        AllCanons = new Canon[RightSide.Length + RightSide.Length];
        RightSide.CopyTo(AllCanons, 0);
        LeftSide.CopyTo(AllCanons, RightSide.Length);
    }

    public void FireCanons(ShipSide side)
    {
        // TODO pool the canons wşth reload time and touching somewhere
        foreach (Canon canon in (side == ShipSide.Right) ? RightSide : LeftSide)
        {
            canon.FireCanon();
        }
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
        return RightSide[0].transform.eulerAngles.x;
    }

    public void SetCanonAngle(float diff)
    {
        float angle  = Mathf.Clamp(GetCanonAngle() - (diff * CanonAngleChangeSpeed), 359f - MaxCanonAngle, 359f);
        foreach (Canon canon in AllCanons)
        {
            Vector3 vec = canon.transform.eulerAngles;
            canon.transform.eulerAngles = Vector3.Lerp(canon.transform.eulerAngles, new Vector3(angle, vec.y, vec.z), Time.deltaTime * 20f);
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