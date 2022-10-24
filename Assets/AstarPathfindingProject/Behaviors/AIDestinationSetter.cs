using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		private GameObject player;
		private GameObject ghost;
		public string currTarget;
		public bool Steer;
		IAstarAI ai;

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
			
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update () {
			if(player == null){
			Steer = true;
            player = GameObject.FindWithTag("Player");
			target = player.transform;
			currTarget = "player";
			
		}
		if (Steer == true){
			float dice = Random.Range(0f, 1f);
			if (dice < 0.6f){
				Steer =false;
				player = GameObject.FindWithTag("Player");
				target = player.transform;
				currTarget = "player";
				StartCoroutine(Wait1());
			}
			else if (dice < 0.7f){
				Steer =false;
				player = GameObject.FindWithTag("ghost");
				target = player.transform;
				currTarget = "ghost";
				StartCoroutine(Wait());
			}
			else if (dice < 0.8f){
				Steer =false;
				player = GameObject.FindWithTag("ghosttt");
				target = player.transform;
				currTarget = "ghost";
				StartCoroutine(Wait());
			}
			else if (dice < 0.9f){
				Steer =false;
				player = GameObject.FindWithTag("ghostttt");
				target = player.transform;
				currTarget = "ghost";
				StartCoroutine(Wait());
			}
			else{
				Steer =false;
				player = GameObject.FindWithTag("ghostt");
				target = player.transform;
				currTarget = "ghostt";
				StartCoroutine(Wait());
			}
		}
		if (target != null && ai != null) ai.destination = target.position;
		
	}
	private IEnumerator Wait(){
		yield return new WaitForSeconds(1f);
		target = player.transform;
		Steer = true;
	}
	private IEnumerator Wait1(){
		yield return new WaitForSeconds(3f);
		Steer = true;
	}
}
}