using UnityEngine;

public class Utilities {

	public static Vector3 Vector3Reciprocal (Vector3 vector) {
		return new Vector3 (1 / vector.x, 1 / vector.y, 1 / vector.z);
	}

	public static bool IsInLayerMask(int layer, LayerMask layerMask) {
		return layerMask == (layerMask | (1 << layer));
	}
}
