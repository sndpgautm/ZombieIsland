using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	// Platform moves from PosA to posB
	private Vector3 posA;

	private Vector3 posB;

	private Vector3 nexPos; /*It will be the position depending upon current position 
	for eg. if posA is current posB wwill be nexpos*/  

	[SerializeField] //So that we can edit it from inspector in UNITY
	private float speed;  //Speed of Platform

	[SerializeField]
	//Transform used to store and manipulate the position, rotation and scale of the object.
	private Transform childTransform;//Refrence to FloatingGround
	[SerializeField]
	private Transform transformB; //Refrence to PositionB

	// Use this for initialization
	void Start () {
		posA = childTransform.localPosition; //This is the 1stposition of Platform
		posB = transformB.localPosition;
		nexPos = posB;
		
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		
	}
	private void Move(){
		childTransform.localPosition = Vector3.MoveTowards (childTransform.localPosition, nexPos, speed * Time.deltaTime);
		// Here platform moves from localPostiton to nextpos with a speed
		 // deltaTime "The time in seconds it took to complete the last frame" 
		if (Vector3.Distance (childTransform.localPosition, nexPos) <= 0.1) {
			// It executes if distance between current positon of platform and nexPos is less or euals to 0.1 
			MoveBack ();
		}
		
	}
	private void MoveBack (){
		nexPos = nexPos != posA ? posA : posB;
		//nexPos is eualto posA or posB based on codition (nexPos != posA ?)
		//Inside conditon, if nexPos isn't equalto  posA it uses posA if it euals then uses posB
	}
}
