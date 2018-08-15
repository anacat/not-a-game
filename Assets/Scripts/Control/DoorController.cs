using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour 
{
	public string sceneToLoad;
	public Animator fadeAnimator;
	public BoolVariable canPlayerMove;

	private AudioSource _audioSource;

	public void OpenDoor()
	{
		StartCoroutine(SceneTransition());
	}

	public IEnumerator SceneTransition()
    {
        AsyncOperation asyncOperation;

        DontDestroyOnLoad(gameObject);
		//_audioSource.Play();

        fadeAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(2);

        asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        fadeAnimator.SetTrigger("FadeOut");

        yield return AnimationHelper.WaitForAnimation(fadeAnimator);

        Destroy(gameObject);
		
		canPlayerMove.SetValue(true);
    }
}
