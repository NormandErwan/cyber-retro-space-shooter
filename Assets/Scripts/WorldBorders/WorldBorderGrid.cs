using UnityEngine;
using System.Collections;
using VolumetricLines;

public class WorldBorderGrid : MonoBehaviour {

	public int gridLineSpaceSize;
	public GameObject BorderGridLine;

	/*
	 * Contains the blueprints to generate the border grids.
	 */
	class BorderGridDefinition {
		public string name;
		public Vector3 localPosition;
		public Quaternion rotation;
		public int worldScaleIndexGridLength, worldScaleIndexLineLength;

		public BorderGridDefinition(string name, Vector3 gridPosition, Vector3 gridRotation,
			int worldScaleIndexGridLength, int worldScaleIndexLineLength) 
		{
			this.name = name;
			this.localPosition = gridPosition;
			this.rotation = Quaternion.Euler(gridRotation);
			this.worldScaleIndexGridLength = worldScaleIndexGridLength;
			this.worldScaleIndexLineLength = worldScaleIndexLineLength;
		}
	}
	BorderGridDefinition[] borderGridDefinitions = new BorderGridDefinition[] {
		new BorderGridDefinition("Front grid", new Vector3(0f, 0f, 0.5f), new Vector3(0f, 180f, 0f), 0, 1),
		new BorderGridDefinition("Behind grid", new Vector3(0f, 0f, -0.5f), new Vector3(0f, 0f, 0f), 0, 1),
		new BorderGridDefinition("Top grid", new Vector3(0f, 0.5f, 0f), new Vector3(90f, 0f, 0f), 0, 2),
		new BorderGridDefinition("Bottom grid", new Vector3(0f, -0.5f, 0f), new Vector3(-90f, 0f, 0f), 0, 2),
		new BorderGridDefinition("Right grid", new Vector3(0.5f, 0f, 0f), new Vector3(0f, -90f, 0f), 2, 1),
		new BorderGridDefinition("Left grid", new Vector3(-0.5f, 0f, 0f), new Vector3(0f, 90f, 0f), 2, 1)
	};

	/*
	 * Generate each border grid.
	 */
	public void GenerateBorderGrids () {
		foreach (BorderGridDefinition borderGridDefinition in borderGridDefinitions) {
			GameObject borderGrid = new GameObject ();
			borderGrid.name = borderGridDefinition.name;

			// Position and orient the grid.
			borderGrid.transform.rotation = borderGridDefinition.rotation; // NOTE: Important to adjust the rotation before set the parent to have the reset good scale
			borderGrid.transform.parent = this.transform;
			borderGrid.transform.localPosition = borderGridDefinition.localPosition;

			GenerateBorderGridLines (borderGrid, borderGridDefinition.worldScaleIndexGridLength, borderGridDefinition.worldScaleIndexLineLength, 
				Quaternion.Euler (90f, 0f, 0f), 0); // Spawn vertical lines along the grid.
			GenerateBorderGridLines (borderGrid, borderGridDefinition.worldScaleIndexLineLength, borderGridDefinition.worldScaleIndexGridLength,
				Quaternion.Euler (0f, 90f, 0f), 1); // Spawn horizontal lines along the grid.
		}
	}

	/*
	 * Generate the lines of a border grid along an axis.
	 */
	void GenerateBorderGridLines(GameObject grid, int worldScaleIndexGridLength, int worldScaleIndexLineLength,
		Quaternion lineLocalRotation, int lineLocalPositionIndex) 
	{
		// Spawn a new line on each gridLineSpaceSize along the worldScaleIndexGridLength axis.
		float gridHalfLength = this.transform.lossyScale [worldScaleIndexGridLength] / 2;
		for (float i = -gridHalfLength; i <= gridHalfLength; i += gridLineSpaceSize) {
			GameObject line = Instantiate (BorderGridLine);
			line.transform.parent = grid.transform;

			// Position and orient the line along the axis.
			Vector3 lineLocalPosition = Vector3.zero;
			lineLocalPosition [lineLocalPositionIndex] += i;
			line.transform.localPosition = lineLocalPosition;
			line.transform.localRotation = lineLocalRotation;

			// Because the z scale is the component of the vector that has the influence on the line length,
			// adjust it in order to make the oriented line taking a lossy scale of one.
			Vector3 lineLocalScale = line.transform.localScale;
			lineLocalScale[2] = line.transform.localScale[worldScaleIndexLineLength];
			line.transform.localScale = lineLocalScale;

			// Adjust the line length in the shader, considering the z scale is one.
			Vector3 lineHalfLength = new Vector3 (0f, 0f, this.transform.lossyScale [worldScaleIndexLineLength]) / 2;
			line.GetComponent<VolumetricLineBehavior> ().SetStartAndEndPoints (-lineHalfLength, lineHalfLength);
		}
	}
}
