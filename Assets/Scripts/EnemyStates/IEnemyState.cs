using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class which uses a interface needs to implement all these functions 
public interface IEnemyState
{

	void Execute();
	void Enter (Enemy enemy); // refers which enemy it controls when it enters the state
	void Exit();
	void OnTriggerEnter(Collider2D other);
}
