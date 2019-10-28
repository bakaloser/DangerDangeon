using UnityEngine;
using UnityEditor;

public class Hero : MonoBehaviour
{
	public RectTransform spriteTr;
	public BulletsHandler bulletHandler;
	[HideInInspector]
	public bool isMovable = true;

	private float speed = 0.4f;
	private void Update()
	{
		if (!isMovable)
			return;

		Move();

		if (Input.GetKeyDown(KeyCode.X))
			bulletHandler.Shoot(transform.position);
	}

	private void Move()
	{
		float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		if (x != 0)
			transform.Translate(new Vector3(x, 0));
		else
		{
			float y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
			if (y != 0)
				transform.Translate(new Vector3(0, y));
		}

		SwitchSprite();
	}

	private void SwitchSprite()
	{
		if (spriteTr != null)
		{
			Vector3 direction = transform.right * Input.GetAxis("Horizontal");
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