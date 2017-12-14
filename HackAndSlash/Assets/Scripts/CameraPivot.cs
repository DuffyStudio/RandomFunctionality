using UnityEngine;
using System.Collections;

public class CameraPivot : MonoBehaviour {

	public Transform character;
	public float speed;
	// Use this for initialization
	void Start () {
	
	}

	void Update () {
		
		transform.position = Vector3.Lerp (transform.position, new Vector3(character.position.x -5,transform.position.y,character.position.z-3), speed * Time.deltaTime);
	}
}
