using System.Collections;
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
    private Transform _cannonballParent;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private SoundController _soundController;

    [SerializeField]
    private GameObject _particleEffect;

    private void Start()
    {
        InitTrajectory();
        Canonball.GravityEffect = _gravityEffect;
        Canonball.InitialSpeed = CannonPower;
        _cannonballParent = GameObject.FindGameObjectWithTag("CanonballParent").transform;
    }

    private void Update() {
        if (ShowTrajectory)
        {
            DrawTrajectory();
        }
    }

    public void FireCanon()
    {
        if (IsReadyToFire)
        {
            Canonball.transform.parent = _cannonballParent;
            Canonball.gameObject.SetActive(true);
            Canonball.FireCannon(Mathf.Deg2Rad * transform.eulerAngles.y, Mathf.Deg2Rad * transform.eulerAngles.x);
            CanonCount --;
            ShowTrajectory = false;
            ToggleTrajectory(false);
            IsReadyToFire = false;

            _audioSource.clip = _soundController.GetRandomSound();
            _audioSource.Play();

            _particleEffect.SetActive(true);

            if (CanonCount > 0)
            {
                StartCoroutine(ResetCanonball());
            }
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

    IEnumerator ResetCanonball()
    {
        yield return new WaitForSeconds(CooldownTime);

        Canonball.ResetCanonball();
        Canonball.transform.SetParent(transform);
        Canonball.transform.localPosition = new Vector3(0,0,5);
        Canonball.transform.localEulerAngles = Vector3.zero;
        Canonball.gameObject.SetActive(false);
        IsReadyToFire = true;

        _particleEffect.SetActive(false);

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