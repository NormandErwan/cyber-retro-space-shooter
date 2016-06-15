using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Text textLog;
	public GameObject PlayerModel;

	void Start () {
		Input.gyro.enabled = true;
	}

	void Update () {
		textLog.text = "" + Input.gyro.rotationRateUnbiased + " " + Input.gyro.attitude.eulerAngles + "" + Input.gyro.rotationRateUnbiased.z;
	}

	void FixedUpdate () {
		Vector3 gyroRot = Input.gyro.rotationRateUnbiased;
		transform.Rotate(-gyroRot.x, -gyroRot.y, 0, Space.Self);
	}
}
