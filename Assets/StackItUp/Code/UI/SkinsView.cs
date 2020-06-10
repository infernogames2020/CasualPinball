using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using FuriousPlay;
public class SkinsView : View
{
	[SerializeField]
	List<ShapeButton> allShapes;
	[SerializeField]
	GridLayoutGroup grid;

	private Vector3 startPosition;
	private ShapeButton currentShape;

	private void Awake()
	{
	}

	private void Start()
	{
		startPosition = transform.localPosition;
		int currentStack = SaveManager.SaveData.currentStack;
		
		foreach(ShapeButton shape in allShapes)
		{
			shape.Unlocked();
			if(shape.name.Equals(currentStack.ToString()))
			{
				currentShape = shape;
				shape.Active();
			}
		}

	}
	public override void Hide()
	{
		transform.DOLocalMoveX(startPosition.x, 0.3f);
	}

	public override void Init(Hashtable data)
	{
		//var stackList = (List<StackData>)data["stacks"];
	}

	public override void Show()
	{
		transform.DOLocalMoveX(0, 0.3f);
		int current = SaveManager.SaveData.currentStack;
	}

	public void ShapeSelected(GameObject go)
	{
		if (currentShape != null)
		{
			currentShape.Unlocked();
		}

		currentShape = go.GetComponent<ShapeButton>();
		currentShape.Active();
		ActionManager.TriggerEvent(GameEvents.STACK_CHANGE, new Hashtable() {
			{"stack", int.Parse(currentShape.name)}
		});
	}
}
