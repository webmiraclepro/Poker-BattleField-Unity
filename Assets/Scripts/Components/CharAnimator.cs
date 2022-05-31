using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimator : MonoBehaviour
{
    private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    public IEnumerator ShowChar(string charStr)
    {

        GameObject charObj = transform.Find(charStr).gameObject;
        charObj.SetActive(true);

        _animation.PlayQueued("Appearing");

        yield return WaitForAnimation(_animation);

        charObj.SetActive(false);
    }

    private IEnumerator WaitForAnimation(Animation anim)
    {
        do
        {
            yield return null;
        }
        while (anim.isPlaying);
    }
}
