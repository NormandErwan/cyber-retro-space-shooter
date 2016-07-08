using UnityEngine;
using System.Collections;

public class DebrisSpawner : MonoBehaviour {

	public WorldBorders worldBorders;
	public Vector3 spawnBoxSize;
	public GameObject[] debris;
	public float debrisSpeed;

	Vector3[, ,] spawnBoxCenterGrid;

	void Update () {
		if (spawnBoxCenterGrid == null && worldBorders.Ready) {
			SetupSpawnGrid ();
			GenerateDebris ();
		}
	}

	void SetupSpawnGrid () {
		// Allocate
		Vector3 spawnGridSize = Vector3.Scale (worldBorders.BordersMax - worldBorders.BordersMin, Vector3Reciprocal(spawnBoxSize));
		spawnBoxCenterGrid = new Vector3[(int)spawnGridSize.x, (int)spawnGridSize.y, (int)spawnGridSize.z];

		// Fill
		for (int i = 0; i < spawnBoxCenterGrid.GetLength(0); i++) {
			for (int j = 0; j < spawnBoxCenterGrid.GetLength(1); j++) {
				for (int k = 0; k < spawnBoxCenterGrid.GetLength(2); k++) {
					spawnBoxCenterGrid [i, j, k] = Vector3.Scale (new Vector3 (i, j, k), spawnBoxSize) 
						+ spawnBoxSize / 2 // center of the box
						+ worldBorders.BordersMin;
				}
			}
		}
	}

	void GenerateDebris () {
		Vector3 debrisSpeedForce = transform.forward * debrisSpeed;

		foreach (Vector3 spawnBoxCenter in spawnBoxCenterGrid) {
			int randomDebrisIndex = Random.Range (0, debris.Length-1);

			GameObject deb = Instantiate<GameObject> (debris[randomDebrisIndex]);
			deb.transform.SetParent (this.transform);
			deb.transform.position = spawnBoxCenter;

			deb.GetComponent<DebrisController> ().SpeedForce = debrisSpeedForce;
			deb.GetComponent<DebrisController> ().ConfigurateDebris();
		}
	}

	void OnDrawGizmosSelected() {
		if (!worldBorders.Ready) {
			return;
		}

		SetupSpawnGrid ();
		foreach (Vector3 spawnBoxCenter in spawnBoxCenterGrid) {
			Gizmos.DrawWireCube (spawnBoxCenter, spawnBoxSize);
		}
	}

	Vector3 Vector3Reciprocal (Vector3 vector) {
		return new Vector3 (1 / vector.x, 1 / vector.y, 1 / vector.z);
	}
}
