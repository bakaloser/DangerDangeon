using UnityEngine;
using UnityEditor;

public class Hero : MonoBehaviour
{
	public RectTransform spriteTr;
	public BulletsHandler bulletHandler;
	[HideInInspector]
	public bool isMovable = true;

	private float speed = 0.4f;
	private float fixedMoveFloat = 0.01f;
	private float curMoveFloat = 0;

	public void MoveBtnLeft()
	{
		fixedMoveFloat = -0.01f;
		curMoveFloat += fixedMoveFloat;
	}

	public void MoveBtnRight()
	{
		fixedMoveFloat = 0.01f;
		curMoveFloat += fixedMoveFloat;
	}

	public void MoveBtnReset()
	{
		curMoveFloat = 0;
	}

	public void ShootBtn()
	{
		bulletHandler.Shoot(transform.position);
	}

	private void Update()
	{
		if (!isMovable)
			return;

		if (curMoveFloat == 0)
			Move(Input.GetAxis("Horizontal"), 0);//Input.GetAxis("Vertical"));
		else
		{
			Move(curMoveFloat, 0);
			curMoveFloat += fixedMoveFloat;
		}

		if (Input.GetKeyDown(KeyCode.X))
			bulletHandler.Shoot(transform.position);
	}

	private void Move(float dx, float dy)
	{
		float x = dx * speed * Time.deltaTime;
		if (x != 0)
			transform.Translate(new Vector3(x, 0));
		else
		{
			float y = dy * speed * Time.deltaTime;
			if (y != 0)
				transform.Translate(new Vector3(0, y));
		}

		SwitchSprite();
	}

	private void SwitchSprite()
	{
		if (spriteTr != null)
		{
			Vector3 direction = Input.GetAxis("Horizontal") != 0 ? transform.right * Input.GetAxis("Horizontal") : transform.right * curMoveFloat;
			if (direction.x > 0)
				spriteTr.localRotation = new Quaternion(0, 180, 0, 0);
			else
				spriteTr.localRotation = new Quaternion(0, 0, 0, 0);
		}
	}

	public void ResetPosition()
	{
		transform.localPosition = new Vector3(-407f, -898f, 0);
		spriteTr.localRotation = new Quaternion(0, 0, 0, 0);
	}
}