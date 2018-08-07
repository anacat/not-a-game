using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public BoolVariable canPlayerMove;
	public AnimationCurve movementCurve;

	private void Update()
	{
		//strict clicks
		if(Input.GetButtonDown("Fire1") && canPlayerMove.GetValue())
		{
			StartCoroutine(MovePlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
		}
	}

	public IEnumerator MovePlayer(Vector2 position)
	{
		canPlayerMove.SetValue(false);

		float interpolationTime = 1f;
		float timeStarted;
		float deltaTime = 0;
		float percentageDone = 0;
		Vector2 initPosition = transform.localPosition;
		
		timeStarted = Time.time;

		while(percentageDone < 1f)
		{
			deltaTime = Time.time - timeStarted;
			percentageDone = deltaTime / interpolationTime;

			transform.localPosition = Vector2.Lerp(initPosition, position, movementCurve.Evaluate(percentageDone));

			yield return null;
		}

		canPlayerMove.SetValue(true);
	}
}
