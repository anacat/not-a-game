using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour 
{
	public GameController controller;
	public BoolVariable canPlayerMove;

	private void OnMouseDown()
	{
		if(canPlayerMove.GetValue()) 
		{
			StartCoroutine(Move());
		}
	}

	private IEnumerator Move()
	{
		yield return controller.player.MovePlayer(transform.localPosition);
	}
}
