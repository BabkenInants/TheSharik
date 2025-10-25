using DG.Tweening;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    private bool rotate = true;

    private void Update()
    {
        if (rotate)
            transform.Rotate(direction * speed * Time.deltaTime);
    }

    public void ResetPos()
    {
        rotate = false;
        DOTween.Sequence()
            .Append(transform.DORotate(Vector3.zero, 1f))
            .AppendCallback(RecoverRotation);
    }

    private void RecoverRotation() => rotate = true;
}