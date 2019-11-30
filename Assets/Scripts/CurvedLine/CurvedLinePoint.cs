using UnityEngine;
using UnityEngine.Events;

public class CurvedLinePoint : MonoBehaviour 
{
	public UnityAction FireOrderEvent;

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ship")
        {
			if (other.GetComponent<DamageController>() && other.GetComponent<DamageController>().ShipController.IsPlayer)
			{
				FireOrderEvent?.Invoke();
			}
        }
    }
}