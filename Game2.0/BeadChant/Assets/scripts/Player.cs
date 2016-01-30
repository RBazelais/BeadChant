﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour {

	public float speed = 1000.0F;
	public int maxLength;
	SpriteRenderer sr;

	List<GameObject> beads = new List<GameObject>();
	Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();

	
	
	}

	// Update is called once per frame
	void Update () {

			Vector2 dir = Vector3.zero;
			dir.y = Input.acceleration.y;
			dir.x = Input.acceleration.x;
			if (dir.sqrMagnitude > 1)
				dir.Normalize();
			
			
			rb2d.velocity = speed * dir;

	}



	void  OnTriggerEnter2D(Collider2D other){
		GameObject tempBead;
		//ifthe player encounters a bead aaddd the bead to the bead array
		//and attach the bead to the correct rigidbody
		if(other.gameObject.CompareTag("Bead")){

			tempBead = other.gameObject;
			//check if the bead is already on the string
			if(!tempBead.GetComponent<Bead>().isActive){
				return;
			}

			/**if there are no beads in the array then attach the bead to the playerand add it to the array*/
			if(beads.Count == 0){
				tempBead.GetComponent<Bead>().AttachDistanceJoint(rb2d);
				beads.Add(tempBead);
			}

			/**if there are beads in the array attach the bead the player
			 and change the rigidbody of the previous bead to the current bead */
			//if you've encountered a bead and you're not at your maxLength...
			else if(beads.Count != maxLength){
				//
				tempBead.GetComponent<Bead>().AttachDistanceJoint(rb2d);

				//Go to the last index and return 1 from that slot in thhe previousBead array
				GameObject[] previousBead = beads.GetRange(beads.Count-1, 1).ToArray();
				//
				previousBead[0].GetComponent<Bead>().AttachDistanceJoint(tempBead.GetComponent<Bead>().rb2d);
				beads.Add(tempBead);


			}

			else if(beads.Count == maxLength){
				//Give me the element of the first thing
				GameObject[] firstBead = beads.GetRange(0,1).ToArray();
				//
				Destroy(firstBead[0]);
				GameObject[] previousBead = beads.GetRange(beads.Count-1, 1).ToArray();
				//
				previousBead[0].GetComponent<Bead>().AttachDistanceJoint(tempBead.GetComponent<Bead>().rb2d);
				tempBead.GetComponent<Bead>().AttachDistanceJoint(rb2d);
				beads.Add(tempBead);

			}

			ColorUpdate();


		}


	}

	void ColorUpdate(){
		float redAmount=0;
		float greenAmount=0;
		float blueAmount=0;
		Color beadColor;
		Color playerColor = Color.white;

		foreach (GameObject bead in beads) {
			beadColor = bead.GetComponent<Bead>().GetColor ();
			Debug.Log (beadColor);
			redAmount+=beadColor[0];
			greenAmount += beadColor[1];
			blueAmount += beadColor[2];

			
		}

		playerColor.r = redAmount / beads.Count;
		playerColor.g = greenAmount / beads.Count;
		playerColor.b = blueAmount / beads.Count;




		Debug.Log (playerColor);
		sr.color = playerColor;




	
		
	}
}
