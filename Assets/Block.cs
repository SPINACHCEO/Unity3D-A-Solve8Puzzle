using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	PuzzleVisualizer c;
	int id;

	public void Setup(PuzzleVisualizer pc, int i){
		c = pc;
		id = i;
	}

	public void Fire(){
		if(c)c.Move (id);
	}
}
