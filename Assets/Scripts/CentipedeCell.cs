using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using static FireCentipede;

public class CentipedeCell : MonoBehaviour
{
	public enum Direction { Left, Right, Down};
	public Image image;
	public Sprite headSprite;
	public Sprite bodySprite;

	[HideInInspector]
	public bool isMovable = false;
	[HideInInspector]
	public int parentCentipedeIndex;
	public CellType Type
	{
		get => type;
		set
		{
			type = value;
			SetSprite(value);
		}
	}

	[HideInInspector]
	public Direction curDir = Direction.Right;
	[HideInInspector]
	public Vector3 prevPos;

	private CellType type;
	private Direction prevDir;

	private void Start()
	{
		prevPos = transform.localPosition;
	}

	private void SetSprite(CellType cellType)
	{
		switch(cellType)
		{
			case CellType.Head:
				image.sprite = headSprite;
				break;
			case CellType.Body:
			default:
				image.sprite = bodySprite;
				break;
		}
	}

	private void Update()
	{
		if (isMovable)
		{
			float moveValue = FireCentipede.instance.speed * 20;
			Vector3 target = FireCentipede.instance.GetNextPosition(this);
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, moveValue);

			if (Vector3.Distance(transform.localPosition, target) < 0.001f)
			{
				prevPos = transform.localPosition;
				if(curDir == Direction.Down)
					curDir = prevDir == Direction.Left ? Direction.Right : Direction.Left;
			}

		}
	}

	public void IsHit()
	{
		Main.instance?.CreatePumpkin(transform.position);
		FireCentipede.instance.DestroyCell(this);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name.Contains("Left") || collision.name.Contains("Right") || collision.name.Contains("Pumpkin"))
		{
			prevDir = curDir;
			curDir = Direction.Down;
		}

		if (collision.name.Contains("Bottom"))
		{
			Main.instance?.IncreaseXp();
			if (Main.instance?.xp == 0)
				Main.instance?.ShowMessage();
			else
			{
				FireCentipede.instance.RemoveAll();
				Main.instance?.RespawnCentipede();
			}
		}
	}
}