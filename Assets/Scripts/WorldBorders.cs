using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldBorders : MonoBehaviour {

	public float borderMarginsPercentage;
	public RenderTexture cameraBorderTextureModel;

	private Vector3 bordersMin, bordersMax;

	private class Border {
		public string name;
		public Vector3 planeScale, planeRotation, planePosition, cameraRotation, cameraPosition;
		public int cameraOrthographicSizeParentLocalScaleIndex, camerafarClipPlaneParentLocalScaleIndex;

		public Border(string name, Vector3 planeScale, Vector3 planePosition, 
					  Vector3 cameraRotation, Vector3 cameraPosition, 
					  int cameraOrthographicSizeParentLocalScaleIndex, int camerafarClipPlaneParentLocalScaleIndex) {
			this.name = name;
			this.planeScale = planeScale;
			this.planeRotation = planeRotation;
			this.planePosition = planePosition;
			this.cameraPosition = cameraPosition;
			this.cameraRotation = cameraRotation;
			this.cameraOrthographicSizeParentLocalScaleIndex = cameraOrthographicSizeParentLocalScaleIndex;
			this.camerafarClipPlaneParentLocalScaleIndex = camerafarClipPlaneParentLocalScaleIndex;
		}
	}
	private Border[] borderDefinitions = new Border[] {
		new Border("Up", new Vector3 (1f, 1f, 0.001f), new Vector3(0f, 0f, 0.5f), new Vector3(0f, 0f, 180f), new Vector3(0f, 0f, -0.5f), 0, 2),
		new Border("Down", new Vector3 (1f, 1f, 0.001f), new Vector3(0f, 0f, -0.5f), new Vector3(0f, 180f, 0f), new Vector3(0f, 0f, 0.5f), 0, 2),
		new Border("Top", new Vector3 (1f, 0.001f, 1f), new Vector3(0f, 0.5f, 0f), new Vector3(-90f, 180f, 0f), new Vector3(0f, -0.5f, 0f), 2, 1),
		new Border("Bottom", new Vector3 (1f, 0.001f, 1f), new Vector3(0f, -0.5f, 0f), new Vector3(90f, 180f, 0f), new Vector3(0f, 0.5f, 0f), 2, 1),
		new Border("Left", new Vector3 (0.001f, 1f, 1f), new Vector3(0.5f, 0f, 0f), new Vector3(0f, 90f, 0f), new Vector3(-0.5f, 0f, 0f), 1, 0),
		new Border("Right", new Vector3 (0.001f, 1f, 1f), new Vector3(-0.5f, 0f, 0f), new Vector3(0f, -90f, 0f), new Vector3(0.5f, 0f, 0f), 1, 0)
	};

	void Start () {
		SetupBorders ();
	}

	/*
	 * Set the world borders following the position and the scale of the object.
	 */
	void SetupBorders () {
		// Used by OnTriggerExit
		bordersMin = - transform.localScale / 2 + transform.position;
		bordersMax = transform.localScale / 2 + transform.position;

		// Setup the box 
		GameObject cameraModel = new GameObject ();
		cameraModel.AddComponent<Camera> ();
		cameraModel.GetComponent<Camera> ().orthographic = true;
		cameraModel.GetComponent<Camera> ().aspect = 1f;
		cameraModel.GetComponent<Camera> ().nearClipPlane = 0f;
		cameraModel.GetComponent<Camera> ().cullingMask = 1 << 0;

		GameObject planeModel = GameObject.CreatePrimitive (PrimitiveType.Cube);
		planeModel.GetComponent<Collider> ().enabled = false;

		foreach (Border border in borderDefinitions) {
			RenderTexture targetTexture = Instantiate<RenderTexture> (cameraBorderTextureModel);
			Material targetMaterial = new Material (Shader.Find ("Standard"));
			targetMaterial.mainTexture = targetTexture;

			GameObject camera = Instantiate<GameObject> (cameraModel);
			camera.name = border.name + " Camera";
			camera.transform.SetParent (this.transform);
			camera.transform.localRotation = Quaternion.Euler (border.cameraRotation);
			camera.transform.localPosition = border.cameraPosition * borderMarginsPercentage;
			camera.GetComponent<Camera> ().orthographicSize = transform.localScale[border.cameraOrthographicSizeParentLocalScaleIndex] / 2 * borderMarginsPercentage;
			camera.GetComponent<Camera> ().farClipPlane = transform.localScale[border.camerafarClipPlaneParentLocalScaleIndex] * (borderMarginsPercentage*.99f);
			camera.GetComponent<Camera> ().targetTexture = targetTexture;

			GameObject plane = Instantiate<GameObject> (planeModel);
			plane.name = border.name;
			plane.transform.SetParent (this.transform);
			plane.transform.localScale = border.planeScale * borderMarginsPercentage;
			plane.transform.localPosition = border.planePosition * borderMarginsPercentage;
			plane.GetComponent<MeshRenderer> ().material = targetMaterial;
		}

		Destroy (cameraModel);
		Destroy (planeModel);
	}

	/*
	 * Constraint every object visible by the player in the box, by translating objects at the other side when they 
	 * exit the box.
	 */
	void OnTriggerExit (Collider other) {
		if (other.tag == "FollowWorldBorders") {
			Vector3 translation = new Vector3 (0, 0, 0);
			Vector3 otherPosition = other.transform.position;

			for (int i = 0; i <= 2; i++) { // For each x,y,z axis
				if (otherPosition [i] > bordersMax [i]) {
					translation [i] -= transform.localScale [i];
				} else if (otherPosition [i] < bordersMin [i]) {
					translation [i] += transform.localScale [i];
				}
			}
			other.transform.Translate (translation, Space.World);
		}
	}
}
