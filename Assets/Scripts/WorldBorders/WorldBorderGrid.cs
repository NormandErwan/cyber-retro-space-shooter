using UnityEngine;
using System.Collections;

public class WorldBorderGrid : MonoBehaviour {

	public RenderTexture cameraBorderTextureModel;
	public LayerMask cameraBorderLayerMask;

	private float worldBordersMarginPercentage;

	class Border {
		public string name;
		public Vector3 planeScale, planePosition, cameraRotation;
		public int cameraOrthographicSizeParentLocalScaleIndex, camerafarClipPlaneParentLocalScaleIndex;

		public Border(string name, Vector3 planeScale, Vector3 planePosition, Vector3 cameraRotation,
			int cameraOrthographicSizeParentLocalScaleIndex, int camerafarClipPlaneParentLocalScaleIndex) {
			this.name = name;
			this.planeScale = planeScale;
			this.planePosition = planePosition;
			this.cameraRotation = cameraRotation;
			this.cameraOrthographicSizeParentLocalScaleIndex = cameraOrthographicSizeParentLocalScaleIndex;
			this.camerafarClipPlaneParentLocalScaleIndex = camerafarClipPlaneParentLocalScaleIndex;
		}
	}
	Border[] borderDefinitions = new Border[] {
		new Border("Up", new Vector3 (1f, 1f, 0.001f), new Vector3(0f, 0f, 0.5f), new Vector3(0f, 0f, 180f), 0, 2),
		new Border("Down", new Vector3 (1f, 1f, 0.001f), new Vector3(0f, 0f, -0.5f), new Vector3(0f, 180f, 0f), 0, 2),
		new Border("Top", new Vector3 (1f, 0.001f, 1f), new Vector3(0f, 0.5f, 0f), new Vector3(-90f, 180f, 0f), 2, 1),
		new Border("Bottom", new Vector3 (1f, 0.001f, 1f), new Vector3(0f, -0.5f, 0f), new Vector3(90f, 180f, 0f), 2, 1),
		new Border("Left", new Vector3 (0.001f, 1f, 1f), new Vector3(0.5f, 0f, 0f), new Vector3(0f, 90f, 0f), 1, 0),
		new Border("Right", new Vector3 (0.001f, 1f, 1f), new Vector3(-0.5f, 0f, 0f), new Vector3(0f, -90f, 0f), 1, 0)
	};

	/*
	 * Setup each border and each camera of the box.
	 */
	public void SetupBorders (float borderMarginsPercentage) {
		GameObject cameraModel = new GameObject ();
		cameraModel.AddComponent<Camera> ();
		cameraModel.GetComponent<Camera> ().orthographic = true;
		cameraModel.GetComponent<Camera> ().aspect = 1f;
		cameraModel.GetComponent<Camera> ().nearClipPlane = 0f;
		cameraModel.GetComponent<Camera> ().cullingMask = cameraBorderLayerMask;

		GameObject planeModel = GameObject.CreatePrimitive (PrimitiveType.Cube);
		planeModel.GetComponent<Collider> ().enabled = false;

		foreach (Border border in borderDefinitions) {
			RenderTexture targetTexture = Instantiate<RenderTexture> (cameraBorderTextureModel);
			Material targetMaterial = new Material (Shader.Find ("Standard"));
			targetMaterial.mainTexture = targetTexture;

			GameObject camera = Instantiate<GameObject>  (cameraModel);
			camera.name = border.name + " Camera";
			camera.transform.SetParent (this.transform);
			camera.transform.localRotation = Quaternion.Euler (border.cameraRotation);
			camera.transform.localPosition = -border.planePosition * borderMarginsPercentage;
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
}
