using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{
	public float speed = 0.8f;
	[HideInInspector]
	public bool isMovable = true;

	private void Update()
	{
		if (!isMovable)
			return;
		transform.Translate(new Vector3(0, speed * Time.deltaTime));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Top")
			BulletsHandler.instance.RemoveBullet(this);
		if (collision.name.Contains("CentipedeCell"))
		{
			BulletsHandler.instance.RemoveBullet(this);
			collision.GetComponent<CentipedeCell>().IsHit();
		}
		else if(collision.name.Contains("Pumpkin"))
		{
			BulletsHandler.instance.RemoveBullet(this);
			collision.GetComponent<ObstaclePumpkin>().IncreaseXp();
		}
	}
}