using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public bool moving;
	public UnityEngine.AI.NavMeshAgent agent;
	public Transform cursorRenderer;
	public Animator animationController;
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			agent.SetDestination (cursorRenderer.position);
			agent.Resume ();
			animationController.Play("Take 001");
		} else {
			animationController.StopPlayback ();
		}
	}
}
