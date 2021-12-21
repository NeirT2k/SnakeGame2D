using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
	private int myOrder;
	private Transform head;
	public float overTime = 0.5f;
	private void Start()
	{
		head = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
		for (int i = 0; i < head.GetComponent<MouseFollowScript>().bodyParts.Count;i++)
		{
			if(gameObject == head.GetComponent<MouseFollowScript>().bodyParts[i].gameObject)
			{
				myOrder = i;
			}
		}
	}
	private Vector3 movementVelocity;
	
	private void FixedUpdate()
	{
		if (myOrder == 0)
		{
			transform.position = Vector3.SmoothDamp(transform.position, head.position, ref movementVelocity, overTime);
			transform.up = head.transform.position - transform.position;
		}
		else
		{
			transform.position = Vector3.SmoothDamp(transform.position,
				head.GetComponent<MouseFollowScript>().bodyParts[myOrder - 1].position, ref movementVelocity, overTime);
			transform.up = head.transform.position - transform.position;
		}
	}
	private void Update()
	{ 
	}
}
