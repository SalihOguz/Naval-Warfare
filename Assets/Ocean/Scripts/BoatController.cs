using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoatController : MonoBehaviour{
	[SerializeField] public float m_FinalSpeed = 100F;
	[SerializeField] public float m_InertiaFactor = 0.005F;
	[SerializeField] public float m_turningFactor = 2.0F;
    [SerializeField] public float m_accelerationTorqueFactor = 35F;
	[SerializeField] public float m_turningTorqueFactor = 35F;

	private float m_verticalInput = 0F;
	private float m_horizontalInput = 0F;
    private Rigidbody m_rigidbody;
	private Vector2 m_androidInputInit;

	private float accel=0;
	private float accelBreak;
	private bool _isPlayer;

    void Start()
    {
        _isPlayer = GetComponent<ShipController>().IsPlayer;
        m_rigidbody = GetComponent<Rigidbody>();

        if (!_isPlayer)
		{
        	m_FinalSpeed = m_FinalSpeed * 0.5f;
		}

        accelBreak = m_FinalSpeed * 0.3f;

        initPosition();
    }

	public void initPosition()	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		m_androidInputInit.x = Input.acceleration.y;
		m_androidInputInit.y = Input.acceleration.x;
		#endif
	}


	void Update()	{
		if (_isPlayer)
		{
			setInputs (Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
		}
		else
		{
			setInputs(1f, 0);
		}
	}

	public void setInputs(float iVerticalInput, float iHorizontalInput)	{
		m_verticalInput = iVerticalInput;
		m_horizontalInput = iHorizontalInput;
	}

	 void FixedUpdate()	{
		//base.FixedUpdate();

		if(m_verticalInput>0) {
			if(accel<m_FinalSpeed) { accel+=(m_FinalSpeed * m_InertiaFactor); accel*=m_verticalInput;}
		} else if(m_verticalInput==0) {
			if(accel>0) { accel-=m_FinalSpeed * m_InertiaFactor; }
			if(accel<0) { accel+=m_FinalSpeed * m_InertiaFactor; }
		}else if(m_verticalInput<0){
			if(accel>-accelBreak) { accel-=m_FinalSpeed * m_InertiaFactor*2;  }
		}
		
		m_rigidbody.AddRelativeForce(Vector3.forward  * accel);

        m_rigidbody.AddRelativeTorque(
			m_verticalInput * -m_accelerationTorqueFactor,
			m_horizontalInput * m_turningFactor,
			m_horizontalInput * -m_turningTorqueFactor
        );
    }

	static float Lerp (float from, float to, float value) {
		if (value < 0.0f) return from;
		else if (value > 1.0f) return to;
		return (to - from) * value + from;
	}

}
