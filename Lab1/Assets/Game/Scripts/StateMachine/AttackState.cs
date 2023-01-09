using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{   
    float timer;

    public void OnEnter(Enemy enemy)
    {
        // if(enemy.target != null)
        // {
        //     enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
        //     enemy.StopMoving();
        //     enemy.Attack();
        // }
    }

    public void OnExcute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if(timer >= 1.5f)
        {
            enemy.ChangeState(new IdleState()); 
        }
    }

    public void OnExit(Enemy enemy)
    {
        enemy.StopMoving();
    }
}
