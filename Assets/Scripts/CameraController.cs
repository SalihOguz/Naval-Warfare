using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject ship;
	public bool followShip = false;

	void Update () {
		if (followShip)
		{
			transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y, -100);
		}
	}
}