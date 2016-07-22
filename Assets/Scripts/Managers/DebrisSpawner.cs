using UnityEngine;
using System.Collections;

public class DebrisSpawner : MonoBehaviour {

	public Vector3 spawnBoxSize;
	public GameObject[] debris;
	public float debrisSpeed;

	Vector3[, ,] spawnBoxGrid;

	void Start () {
		SetupSpawnGrid ();
		InstantiateDebris ();
	}

	/*
	 * Display a vizualisation of the spawn boxes.
	 */
	void OnDrawGizmosSelected() {
		SetupSpawnGrid ();

		foreach (Vector3 spawnBox in spawnBoxGrid) {
			Gizmos.DrawWireCube (spawnBox, spawnBoxSize);
		}
	}

	/*
	 * Generate the spawn box grid.
	 */
	void SetupSpawnGrid () {
		Vector3 bordersMin = - transform.localScale / 2 + transform.localPosition;
		Vector3 bordersMax = transform.localScale / 2 + transform.localPosition;

		// Allocate the grid
		Vector3 spawnGridSize = Vector3.Scale (bordersMax - bordersMin, Utilities.Vector3Reciprocal(spawnBoxSize));
		spawnBoxGrid = new Vector3[(int)spawnGridSize.x, (int)spawnGridSize.y, (int)spawnGridSize.z];

		// Fill the grid with the position of the spawn boxes
		for (int i = 0; i < spawnBoxGrid.GetLength(0); i++) {
			for (int j = 0; j < spawnBoxGrid.GetLength(1); j++) {
				for (int k = 0; k < spawnBoxGrid.GetLength(2); k++) {
					spawnBoxGrid [i, j, k] = Vector3.Scale (new Vector3 (i, j, k), spawnBoxSize) 
						+ spawnBoxSize / 2 // center of the box
						+ bordersMin;
				}
			}
		}
	}

	/*
	 * Instantiate a debris along the grid in each spawn box.
	 */
	void InstantiateDebris () {
		// Create the parent object of the generated debris
		GameObject debrisList = new GameObject ("DebrisList");
		debrisList.transform.localScale = Vector3.one;
		debrisList.transform.parent = this.transform;

		Vector3 debrisSpeedForce = transform.forward * debrisSpeed;

		// Instantiate the debris
		foreach (Vector3 spawnBoxCenter in spawnBoxGrid) {
			int randomDebrisIndex = Random.Range (0, debris.Length-1);

			GameObject deb = Instantiate<GameObject> (debris[randomDebrisIndex]);
			deb.transform.SetParent (debrisList.transform);
			deb.GetComponent<DebrisController> ().ConfigurateDebris(debrisSpeedForce);

			Vector3 randomPositionInsideSpawnBox = (spawnBoxSize - deb.transform.lossyScale) / 2; // Assume the debris' scale is smaller than the box' scale
			deb.transform.position = spawnBoxCenter + Vector3.Scale(Random.insideUnitSphere, randomPositionInsideSpawnBox);
		}
	}
}
