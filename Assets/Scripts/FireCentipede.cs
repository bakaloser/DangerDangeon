using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FireCentipede : MonoBehaviour
{
	public static FireCentipede instance;
	public static float headNextDY;
	public enum CellType { Head, Body };
	public GameObject cellPrefab;
	public float speed = 2f;

	[HideInInspector]
	public Dictionary<int, List<CentipedeCell>> centipedeDictionary;
	public enum Direction { Left, Right, Down };
	[HideInInspector]
	public float cellSpace = 0;

	private void Start()
	{
		instance = this;
		centipedeDictionary = new Dictionary<int, List<CentipedeCell>>();
	}

	public void CreateCentipede(int cellsCount, int centipedeIndex)
	{
		float cellHeigth = cellPrefab.GetComponent<RectTransform>().rect.height;
		cellSpace = cellHeigth - cellHeigth / 5;

		List<CentipedeCell> cells = new List<CentipedeCell>();
		for(int i = 0; i < cellsCount; i++)
		{
			CentipedeCell cell = Instantiate(cellPrefab, transform).GetComponent<CentipedeCell>();
			cell.transform.localPosition = new Vector3(cellHeigth + i * cellSpace, -50, 0);
			cell.Type = CellType.Body;
			cells.Insert(0, cell);
		}
		cells[0].Type = CellType.Head;
		cells.ForEach(x => x.isMovable = true);
		cells.ForEach(x => x.parentCentipedeIndex = centipedeIndex);
		centipedeDictionary[centipedeIndex] = cells;
	}

	public void DestroyCell(CentipedeCell cell)
	{
		int centipedeIndex = cell.parentCentipedeIndex;
		if (centipedeDictionary.ContainsKey(centipedeIndex))
		{
			List<CentipedeCell> cells = centipedeDictionary[centipedeIndex];
			int index = cells.IndexOf(cell);
			if (index >= 0)
			{
				if (cell.Type == CellType.Head && cells.Count == 1)
				{
					centipedeDictionary.Remove(centipedeIndex);
					if (centipedeDictionary.Keys.Count == 0)
						Main.instance?.AddCentipede(centipedeIndex);
				}
				else
				{
					if (cell.Type == CellType.Head)
					{
						cells[index + 1].Type = CellType.Head;
						cells.RemoveAt(index);

					}
					else if (index < cells.Count - 1)
					{
						int nextCentipedeIndex = centipedeDictionary.Keys.Count;
						centipedeDictionary.Remove(centipedeIndex);
						List<CentipedeCell> list1 = new List<CentipedeCell>();
						List<CentipedeCell> list2 = new List<CentipedeCell>();
						for (int i = 0; i < cells.Count; i++)
						{
							if (i < index)
							{
								if (i == 0)
									cells[i].Type = CellType.Head;
								else
									cells[i].Type = CellType.Body;
								cells[i].parentCentipedeIndex = centipedeIndex;
								list1.Add(cells[i]);
							}
							else if (i != index)
							{
								if (i == cells.Count - 1)
									cells[i].Type = CellType.Head;
								else
									cells[i].Type = CellType.Body;
								cells[i].parentCentipedeIndex = nextCentipedeIndex;
								list2.Insert(0, cells[i]);
							}
						}
						centipedeDictionary[centipedeIndex] = list1;
						centipedeDictionary[nextCentipedeIndex] = list2;
					}
					else
						cells.RemoveAt(index);
				}
				Destroy(cell.gameObject);
			}
			else
			{
				Destroy(cell.gameObject);
				foreach (int key in centipedeDictionary.Keys)
				{
					for (int i = 0; i < centipedeDictionary[key].Count; i++)
					{
						if (centipedeDictionary[key][i] == null)
							centipedeDictionary[key].RemoveAt(i);
					}
					if (centipedeDictionary[key].Count == 0)
						centipedeDictionary.Remove(key);
				}

			}
		}
	}

	public void StopAll()
	{
		foreach (int index in centipedeDictionary.Keys)
		{
			List<CentipedeCell> cells = centipedeDictionary[index];
			for (int i = 0; i < cells.Count; i++)
				cells[i].isMovable = false;
		}
	}

	public void RemoveAll()
	{
		foreach (int index in centipedeDictionary.Keys)
		{
			List<CentipedeCell> cells = centipedeDictionary[index];
			for (int i = 0; i < cells.Count; i++)
				if (cells[i] != null)
					Destroy(cells[i].gameObject);
		}
		centipedeDictionary = new Dictionary<int, List<CentipedeCell>>();
	}

	public Vector3 GetNextPosition(CentipedeCell cell)
	{
		if (cell.Type != CellType.Head)
		{
			if (centipedeDictionary.ContainsKey(cell.parentCentipedeIndex))
			{
				List<CentipedeCell> cells = centipedeDictionary[cell.parentCentipedeIndex];
				int index = cells.IndexOf(cell);
				if (index >= 0)
					return cells[index - 1].prevPos;
			}
		}
		else
		{
			if (cell.curDir == CentipedeCell.Direction.Left)
				return new Vector3(cell.prevPos.x - cellSpace, cell.prevPos.y, 0);
			else if (cell.curDir == CentipedeCell.Direction.Right)
				return new Vector3(cell.prevPos.x + cellSpace, cell.prevPos.y, 0);
			else
				return new Vector3(cell.prevPos.x, cell.prevPos.y - cellSpace, 0);
		}
		return cell.transform.localPosition;
	}
}