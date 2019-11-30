using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GunController GunController;
    public BoatController BoatController;
    private Camera _camera;

    private Vector3 lastMousePos;
    public bool IsPlayer = false;

    // Bot
    private bool _increaseAngle = true;

    void Start()
    {
        _camera = Camera.main;

        if (!IsPlayer)
        {
            GunController.ToggleAllTrajectories(true);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        
        if (IsPlayer)
        {
            PlayerUpdate();
        }
        else
        {
            BotUpdate();
        }
    }
    
    private void PlayerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            foreach (ShipSide side in GetActiveSide())
            {
                if (!GunController.IsReadyToFire(side))
                {
                    continue;
                }
                GunController.ToggleTrajectories(side, true);
            }
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            // Check sides constantly
            List<ShipSide> activeSides = GetActiveSide();
            foreach (ShipSide side in (ShipSide[]) Enum.GetValues(typeof(ShipSide)))
            {
                if (!GunController.IsReadyToFire(side))
                {
                    continue;
                }

                if (activeSides.Contains(side))
                {
                    if (!GunController.IsSideTrajectoryActive(side))
                    {
                        GunController.ToggleTrajectories(side, true);
                    }
                }
                else
                {
                    if (GunController.IsSideTrajectoryActive(side))
                    {
                        GunController.ToggleTrajectories(side, false);
                    }
                }
            }

            GunController.SetCanonAngle((Input.mousePosition.y - lastMousePos.y));
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            foreach (ShipSide side in GetActiveSide())
            {
                GunController.ToggleAllTrajectories(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (ShipSide side in GetActiveSide())
            {
                GunController.FireCanons(side, IsPlayer);
                GunController.ToggleAllTrajectories(false);
            }
        }
    }

    private void BotUpdate()
    {
        float currentAngle = GunController.GetCanonAngle();
        if (currentAngle >= 359f)
        {
            _increaseAngle = false;
        }
        else if (currentAngle <= (359f - GunController.MaxCanonAngle))
        {
            _increaseAngle = true;
        }

        if (_increaseAngle)
        {
            GunController.SetCanonAngle(-100f);
        }
        else
        {
            GunController.SetCanonAngle(100f);
        }
    }

    private List<ShipSide> GetActiveSide()
    {
        float angle = CalculateAngle180();
        if (Mathf.Abs(angle) <= 10f || Mathf.Abs(angle) >= 170f)
        {
            return new List<ShipSide> { ShipSide.Right, ShipSide.Left };
        }
        else if (angle < 0)
        {
            return new List<ShipSide> { ShipSide.Left };
        }
        else
        {
            return new List<ShipSide> { ShipSide.Right };
        }
    }

    public float CalculateAngle180()
    {
        Vector3 playerAngle = new Vector3(BoatController.transform.forward.x, 0.0f, BoatController.transform.forward.z);
        Vector3 cameraAngle = new Vector3(_camera.transform.forward.x, 0.0f, _camera.transform.forward.z);

        float angle = Quaternion.FromToRotation(playerAngle, cameraAngle).eulerAngles.y;
        if (angle > 180) { return angle - 360f; }
        return angle;
    }
}