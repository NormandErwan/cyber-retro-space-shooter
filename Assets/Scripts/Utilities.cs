using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public static Vector3 Vector3Reciprocal (Vector3 vector) {
		return new Vector3 (1 / vector.x, 1 / vector.y, 1 / vector.z);
	}
}
