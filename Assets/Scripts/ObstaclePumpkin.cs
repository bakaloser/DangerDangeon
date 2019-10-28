using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObstaclePumpkin : MonoBehaviour
{
	public Image image;

	[Header("Sprite states")]
	public Sprite fullXp;
	public Sprite HalfXp;
	public Sprite MinXp;

	private int xp = 3;

	public void IncreaseXp()
	{
		xp--;
		switch(xp)
		{
			case 3:
				image.sprite = fullXp;
				break;
			case 2:
				image.sprite = HalfXp;
				break;
			case 1:
				image.sprite = MinXp;
				break;
			default:
				Main.instance?.RemovePumpkin(this);
				break;
		}
	}
}