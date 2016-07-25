using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour {

	public GameObject explosionPrefab;
	public float explosionTime = 1f;

	private float rigidbodyDragOnExplosion = 10f;

	public IEnumerator Explode (System.Action<GameObject> callback) {
		GetComponent<Rigidbody> ().drag = rigidbodyDragOnExplosion;
		foreach(Collider collider in GetComponents<Collider> ()) {
			collider.enabled = false;
		}
		transform.FindChild("Model").gameObject.SetActive(false);

		GameObject explosion = (GameObject)Instantiate (explosionPrefab, transform.position, transform.rotation);
		explosion.transform.parent = this.transform;
		explosion.transform.localScale = Vector3.one;

		yield return new WaitForSeconds(explosionTime);
		callback (explosion);

		yield return null;
	}
}
