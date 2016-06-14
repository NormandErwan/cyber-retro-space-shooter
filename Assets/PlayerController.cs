using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 10.0f;

	void Start () {
	
	}

	void Update () {
		Vector3 displacement = transform.forward * speed;
		transform.position += displacement * Time.deltaTime;
	}
}
