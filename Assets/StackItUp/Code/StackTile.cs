using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StackTile : MonoBehaviour
{
	public void PopTile(StackPin stackPin)
	{
		transform.DOLocalMove(stackPin.GetEntryPoint(),0.2f);
	}

	public void PushTile(StackPin stackPin)
	{
		var position = stackPin.CalculateNextPosition(gameObject);

		//stackPin.PushTile(gameObject);
		var sequence = DOTween.Sequence();
		sequence.Append(transform.DOLocalMove(stackPin.GetEntryPoint(), 0.2f));
		sequence.Append(transform.DOLocalMove(position, 0.2f));
		sequence.Play();
	}
}
