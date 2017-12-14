using UnityEngine;
using System.Collections;

public class TouchInput : MonoBehaviour {

	public Transform movementCursor;
	public CharacterMovement characterMovement;
	public SpriteRenderer cursorRenderer;

	void Update()
	{
		if (Input.GetButton ("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.collider.tag == "Terrain") {
					movementCursor.position = new Vector3 (hit.point.x, movementCursor.position.y, hit.point.z);
					characterMovement.moving = true;
					cursorRenderer.enabled = true;
				}
			}
		}
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).deltaPosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.collider.tag == "Terrain") {
					movementCursor.position = new Vector3 (hit.point.x, movementCursor.position.y, hit.point.z);
					characterMovement.moving = true;
					cursorRenderer.enabled = true;
				}
			}
		}
	}
}
