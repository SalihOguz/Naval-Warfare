using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {
    public int CanonCount = 0;
    public bool IsReadyToFire = true;
    public float CooldownTime = 4f;
    public Canonball Canonball;
    public float CannonPower = 100f;
    private Vector3 _gravityEffect = new Vector3(0,-9.81f,0);

    [SerializeField]
    private GameObject _goCanonSmoke;

    [Header ("Trajectory")]
    public bool ShowTrajectory = false;
    public int NumIterations = 100;
    public int IterationsPerDot = 10;
    public Transform TrajectoryParent;
    public GameObject TrajectoryPoint;
    private List<GameObject> _trajectoryList = new List<GameObject>();

    private void Start()
    {
        InitTrajectory();
        Canonball.GravityEffect = _gravityEffect;
        Canonball.InitialSpeed = CannonPower;
    }

    private void Update() {
        if (ShowTrajectory)
        {
            //ToggleTrajectory(true);
            DrawTrajectory();
        }
        // else
        // {
        //     ToggleTrajectory(false);
        // }
    }

    public void FireCanon()
    {
        if (IsReadyToFire)
        {
            Canonball.transform.SetParent(GameObject.FindGameObjectWithTag("CanonballParent").transform);
            Canonball.FireCannon(Mathf.Deg2Rad * transform.eulerAngles.y, Mathf.Deg2Rad * transform.eulerAngles.x);
            CanonCount --;
            ShowTrajectory = false;
            ToggleTrajectory(false);
            IsReadyToFire = false;

        }
    }

    Vector3 ComputeInitialVelocity()
    {
        float sinTheta = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
        float cosTheta = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y);
        float sinPhi = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);
        float cosPhi = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.x);
        
        return CannonPower * new Vector3(cosPhi * sinTheta, -sinPhi, cosPhi * cosTheta);
    }

    #region Trajectory

    void DrawTrajectory()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 velocity = ComputeInitialVelocity();
        Vector3 position = transform.position;

        for (int i = 0; i < NumIterations; ++i)
        {
            velocity += _gravityEffect * dt;
            position += velocity * dt;
            
            if (i % IterationsPerDot != 0)
            continue;
            
            DrawDot(position, (int)(i/IterationsPerDot));
        }
    }

    private void DrawDot(Vector3 position, int index)
    {
        _trajectoryList[index].transform.position = position;
    }

    private void InitTrajectory()
    {
        for (int i = 0; i < (int)(NumIterations/IterationsPerDot); i++)
        {
            GameObject trajectoryObject = Instantiate(TrajectoryPoint, transform.position, Quaternion.identity, TrajectoryParent);
            trajectoryObject.SetActive(false);
            _trajectoryList.Add(trajectoryObject);
        }
    }

    public void ToggleTrajectory(bool isVisible)
    {
        TrajectoryParent.GetComponent<LineRenderer>().enabled = isVisible;
        foreach (GameObject item in _trajectoryList)
        {
            item.SetActive(isVisible);
        }
    }

    public bool IsTrajectoryActive()
    {
        return _trajectoryList[0].activeSelf;
    }

    #endregion
}