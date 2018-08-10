using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public BoolVariable canPlayerMove;
    public AnimationCurve movementCurve;

    private void Update()
    {
        if (CanMovePlayer())
        {
            CheckClick();
        }
    }

    //verifies if player can move and if it's not clicking on UI 
    private bool CanMovePlayer()
    {
        return Input.GetButtonDown("Fire1") && canPlayerMove.GetValue() && !EventSystem.current.IsPointerOverGameObject();
    }

    private void CheckClick()
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(origin, Vector2.zero);

        bool foundGhost = false;

        if (hit.Length > 0)
        {
            foreach (RaycastHit2D c in hit)
            {
                if (c.collider.CompareTag("Ghost"))
                {
                    StartCoroutine(MovePlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.7f));

                    foundGhost = true;
                    break;
                }
                else if (c.collider.CompareTag("CoupleGhost"))
                {
                    AngryCoupleController ag = c.collider.gameObject.GetComponent<AngryCoupleController>();
                    ag.StartStoryTime();

                    foundGhost = true;
                    break;
                }
            }

            //does full movement if it's not a ghost
            if (!foundGhost)
            {
                StartCoroutine(MovePlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1f));
            }
        }
    }

    public IEnumerator MovePlayer(Vector2 position, float percentage)
    {
        canPlayerMove.SetValue(false);

        float interpolationTime = 1f;
        float timeStarted;
        float deltaTime = 0;
        float percentageDone = 0;
        Vector2 initPosition = transform.localPosition;

        timeStarted = Time.time;

        while (percentageDone < percentage)
        {
            deltaTime = Time.time - timeStarted;
            percentageDone = deltaTime / interpolationTime;

            transform.localPosition = Vector2.Lerp(initPosition, position, movementCurve.Evaluate(percentageDone));

            yield return null;
        }

        canPlayerMove.SetValue(true);
    }
}
