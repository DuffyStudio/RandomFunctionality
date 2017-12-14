using UnityEngine;
using System.Collections;

public class MovementCursor : MonoBehaviour {
	public CharacterMovement characterMovement;
	public SpriteRenderer cursorSprite;

	void Start () {
		cursorSprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider mesh){
		if (mesh.tag == characterMovement.tag) {
			if (characterMovement.moving) {
				characterMovement.moving = false;
				cursorSprite.enabled = false;
			}
		}
	}
}
