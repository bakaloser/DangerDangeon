using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BulletsHandler : MonoBehaviour
{
	public static BulletsHandler instance;
	public GameObject bulletPrefab;
	public float shootingDelay = 0.2f;

	[HideInInspector]
	public List<Bullet> bullets;
	private float cooldownTime = 0;

	private void Start()
	{
		instance = this;
		bullets = new List<Bullet>();
	}

	private void Update()
	{		
		cooldownTime -= Time.deltaTime;
	}

	public void Shoot(Vector3 startPos)
	{
		if (cooldownTime <= 0)
		{
			cooldownTime = shootingDelay;
			Bullet bullet = Instantiate(bulletPrefab, startPos, Quaternion.identity, transform).GetComponent<Bullet>();
			bullets.Add(bullet);
		}
	}

	public void RemoveBullet(Bullet bullet)
	{
		bullets.Remove(bullet);
		Destroy(bullet.gameObject);
	}

	public void RemoveAll()
	{
		for (int i = 0; i < bullets.Count; i++)
			Destroy(bullets[i].gameObject);
		bullets = new List<Bullet>();
	}
}