using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrulySpinach : MonoBehaviour {

	public PuzzleVisualizer puzzle;

	TrulyBrainPuzzle brain;

	public void StartSolve(){
		//puzzle.SetVisualizeable (false);
		brain = new TrulyBrainPuzzle ();
		brain.start (puzzle.puzzle, ReceiveSolution);
	}

	void ReceiveSolution(List<int> actions){
		//puzzle.SetVisualizeable (true);
		StartCoroutine (Perform (actions));
	}

	IEnumerator Perform(List<int> actions){
		while (actions.Count > 0) {
			Debug.Log ("perform " + actions[0].ToString());
			puzzle.Move (actions [0]);
			actions.RemoveAt (0);

			yield return new WaitForSeconds (Random.Range(1f,2.5f));
		}
	}
}
