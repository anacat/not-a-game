using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public BoolVariable canPlayerMove;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private int _pointerID = -1;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

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
        if (Input.touchCount > 0)
        {
            _pointerID = Input.GetTouch(0).fingerId;
        }

        return Input.GetButtonDown("Fire1") && canPlayerMove.GetValue() && !EventSystem.current.IsPointerOverGameObject(_pointerID);
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
                    StartCoroutine(MovePlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition)));

                    foundGhost = true;
                    break;
                }
                else if (c.collider.CompareTag("CoupleGhost"))
                {
                    AngryCoupleController ag = c.collider.gameObject.GetComponent<AngryCoupleController>();
                    ag.MoveAndRunStory();

                    foundGhost = true;
                    break;
                }
                else if (c.collider.CompareTag("Door"))
                {
                    _animator.SetTrigger("npcInteract");
                    gameController.doorController.OpenDoor();
                    break;
                }
            }

            if (!foundGhost)
            {
                StartCoroutine(MovePlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            }
        }
    }

    public IEnumerator MovePlayer(Vector2 position)
    {
        canPlayerMove.SetValue(false);
        _animator.SetBool("walking", true);
        
        _audioSource.Play();
        _audioSource.loop = true;

        if (position.x < transform.localPosition.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        while (Vector2.Distance(transform.localPosition, position) > 0)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, position, 0.1f);

            yield return null;
        }

        canPlayerMove.SetValue(true);
        _animator.SetBool("walking", false);

        _audioSource.Stop();
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return _spriteRenderer;
    }
}
