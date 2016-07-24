using UnityEngine;
using System.Collections;
using VolumetricLines;

public class WorldBorderGrid : MonoBehaviour {

	public int gridLineSpaceSize;
	public GameObject BorderGridLine;

	class BorderGridDefinition {
		public string name;
		public Vector3 localPosition;
		public Quaternion localRotation;

		public BorderGridDefinition(string name, Vector3 gridPosition, Vector3 gridRotation) 
		{
			this.name = name;
			this.localPosition = gridPosition;
			this.localRotation = Quaternion.Euler(gridRotation);
		}
	}
	BorderGridDefinition[] borderGridDefinitions = new BorderGridDefinition[] {
		new BorderGridDefinition("Front grid", new Vector3(0f, 0f, 0.5f), new Vector3(0f, 180f, 0f)),
		new BorderGridDefinition("Behind grid", new Vector3(0f, 0f, -0.5f), new Vector3(0f, 0f, 0f)),
		new BorderGridDefinition("Top grid", new Vector3(0f, 0.5f, 0f), new Vector3(90f, 0f, 0f)),
		new BorderGridDefinition("Bottom grid", new Vector3(0f, -0.5f, 0f), new Vector3(-90f, 0f, 0f)),
		new BorderGridDefinition("Right grid", new Vector3(0.5f, 0f, 0f), new Vector3(0f, -90f, 0f)),
		new BorderGridDefinition("Left grid", new Vector3(-0.5f, 0f, 0f), new Vector3(0f, 90f, 0f))
	};

	public void GenerateBorderGrids () {
		foreach (BorderGridDefinition borderGridDefinition in borderGridDefinitions) {
			GameObject borderGrid = new GameObject ();
			borderGrid.name = borderGridDefinition.name;

			borderGrid.transform.parent = this.transform;
			borderGrid.transform.localPosition = borderGridDefinition.localPosition;
			borderGrid.transform.localRotation = borderGridDefinition.localRotation;
			borderGrid.transform.localScale = Vector3.one;

			GenerateBorderGridLines (borderGrid, Quaternion.Euler (90f, 0f, 0f), 0, 1);
			GenerateBorderGridLines (borderGrid, Quaternion.Euler (0f, 90f, 0f), 1, 0);
		}
	}

	void GenerateBorderGridLines(GameObject borderGrid, Quaternion lineOrientation, 
		int borderGridTransformIndexLength, int borderGridTransformIndexLineLength) 
	{
		float gridLineLocalSpaceSize = gridLineSpaceSize / this.transform.lossyScale [borderGridTransformIndexLength];
		for (float i = -0.5f; i <= 0.5f; i = i + gridLineLocalSpaceSize) {
			GameObject borderGridLineVertical = Instantiate (BorderGridLine);
			borderGridLineVertical.transform.parent = borderGrid.transform;

			Vector3 borderGridLineLocalPosition = Vector3.zero;
			borderGridLineLocalPosition [borderGridTransformIndexLength] += i;
			borderGridLineVertical.transform.localPosition = borderGridLineLocalPosition;
			borderGridLineVertical.transform.localRotation = lineOrientation;

			Vector3 borderGridLineHalfLength = new Vector3 (0f, 0f, this.transform.lossyScale [borderGridTransformIndexLineLength]) / 2;
			borderGridLineVertical.GetComponent<VolumetricLineBehavior> ().SetStartAndEndPoints (-borderGridLineHalfLength, borderGridLineHalfLength);
		}
	}
}
