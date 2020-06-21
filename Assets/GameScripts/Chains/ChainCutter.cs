using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCutter : MonoBehaviour
{
	public GameObject bladeTrailPrefab;
	public float minCuttingVelocity = .001f;
	public Rigidbody2D ChainRB;
	public CircleCollider2D ChainCircleCollider;

	private bool isCutting = false;
	private Vector2 previousPosition;
	private GameObject currentBladeTrail;

	private void Awake()
	{
		
	}

	public void UpdateCut(Vector2 newPosition)
	{
		if (isCutting)
		{
			ChainRB.position = newPosition;

			float velocity = (newPosition - previousPosition).magnitude * Time.deltaTime;
			if (velocity > minCuttingVelocity)
			{
				ChainCircleCollider.enabled = true;
			}
			else
			{
				ChainCircleCollider.enabled = false;
			}

			previousPosition = newPosition;
		}		
	}

	public bool IsCutting()
	{
		return isCutting;
	}

	public void StartCutting(Vector2 newPosition)
	{
		isCutting = true;
		ChainRB.position = newPosition;
		transform.position = newPosition;
		previousPosition = newPosition;
		currentBladeTrail = Instantiate(bladeTrailPrefab, transform);
		ChainCircleCollider.enabled = false;
	}

	public void StopCutting()
	{
		isCutting = false;
		currentBladeTrail.transform.SetParent(null);
		Destroy(currentBladeTrail, 2f);
		ChainCircleCollider.enabled = false;
	}
}
