using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
	public static Main instance;
	public Hero hero;
	public GridLayoutGroup bgGridLayout;
	public GameObject stonePrefab;
	public ObstaclePumpkin pumpkinPrefab;
	public Transform pumpkinsField;
	public FireCentipede centipede;
	public GameObject message;
	public Text XPText;
	public Text LvlText;

	[HideInInspector]
	public int xp = 3;
	[HideInInspector]
	public int lvl = 1;
	[HideInInspector]
	public int centipedeCount = 4;

	private int cellsCountH, cellsCountV;
	private List<ObstaclePumpkin> pumpkins;
	private bool isPause = false;

	// Start is called before the first frame update
	void Start()
    {
		instance = this;
		message.SetActive(false);
		XPText.text = "XP: " + xp;
		LvlText.text = "LVL: " + lvl;

		pumpkins = new List<ObstaclePumpkin>();
		cellsCountH = (int)(Screen.width / bgGridLayout.cellSize.x);
		if (Screen.width - cellsCountH * bgGridLayout.cellSize.x > 0)
			cellsCountH++;
		cellsCountV = (int)(Screen.height / bgGridLayout.cellSize.y);
		if (Screen.height - cellsCountV * bgGridLayout.cellSize.y > 0)
			cellsCountV++;

		bgGridLayout.constraintCount = cellsCountH;
		for (int i = 0; i < cellsCountH; i++)
		{
			for (int j = 0; j < cellsCountV; j++)
			{
				Instantiate(stonePrefab, bgGridLayout.transform);
			}
		}

		PumpkinsRandomSpawn(6);
		centipede.CreateCentipede(centipedeCount, 0);
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F5))
			RespawnAll();
		if (isPause && Input.anyKeyDown)
			HideMessage();
    }

	public void ShowMessage()
	{
		isPause = true;
		message.SetActive(true);
		StopAllMove();
	}

	private void HideMessage()
	{
		isPause = false;
		message.SetActive(false);
		RespawnAll();
	}

	#region Pumpkin Scripts
	private void PumpkinsRandomSpawn(int count)
	{
		System.Random r = new System.Random();
		float cell_size = bgGridLayout.cellSize.x;
		for (int i = 0; i < count; i++)
		{
			//generate pumpkin position
			Vector2 v = new Vector2(r.Next(2, cellsCountH - 2) + 0.5f, r.Next(2, cellsCountV - 2) + 0.5f);
			ObstaclePumpkin pumpkin = CreatePumpkin(Vector3.zero);
			pumpkin.transform.localPosition = new Vector3(v.x * cell_size, v.y * cell_size);
		}
	}

	public ObstaclePumpkin CreatePumpkin(Vector3 curPosition)
	{
		ObstaclePumpkin pumpkin = Instantiate(pumpkinPrefab, pumpkinsField).GetComponent<ObstaclePumpkin>();
		pumpkin.transform.position = curPosition;
		pumpkins.Add(pumpkin);
		return pumpkin;
	}
	public void RemovePumpkin(ObstaclePumpkin pumpkin)
	{
		pumpkins.Remove(pumpkin);
		Destroy(pumpkin.gameObject);
	}
	#endregion

	private void RemoveAll()
	{
		foreach(ObstaclePumpkin pumpkin in pumpkins)
		{
			if (pumpkin != null)
				Destroy(pumpkin.gameObject);
		}
		pumpkins = new List<ObstaclePumpkin>();
		hero.bulletHandler.RemoveAll();
		centipede.RemoveAll();
	}

	public void RespawnAll()
	{
		lvl = 1;
		LvlText.text = "LVL: " + lvl;
		xp = 3;
		XPText.text = "XP: " + xp;
		RemoveAll();
		hero.ResetPosition();
		hero.isMovable = true;
		PumpkinsRandomSpawn(6);
		centipede.CreateCentipede(centipedeCount, 0);
	}

	public void StopAllMove()
	{
		hero.isMovable = false;
		BulletsHandler.instance?.bullets?.ForEach(x => x.isMovable = false);
		FireCentipede.instance?.StopAll();
	}

	public void RespawnCentipede()
	{
		centipede.RemoveAll();
		AddCentipede(0);
	}

	public void AddCentipede(int centipedeIndex)
	{
		lvl++;
		LvlText.text = "LVL: " + lvl;
		centipedeCount = 4 + lvl / 15;
		centipede.CreateCentipede(centipedeCount, centipedeIndex);
	}

	public void IncreaseXp()
	{
		xp--;
		XPText.text = "XP: " + xp;
	}
}
