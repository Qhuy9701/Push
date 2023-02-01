using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    private List<GameObject> enemies;

    [SerializeField] private List<Transform> enemyPos;

    [SerializeField] private GameObject enemyPref;

    public static EnemyPoolManager Instance { get ; set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        /*for (int i = 0; i < 5; i++){
            GameObject enemy = Instantiate(enemyPref, transform);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }*/
    }

    
    public GameObject SpawnEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeSelf)
            {
                enemy.transform.position = enemyPos[Random.Range(0, enemyPos.Count)].position;//Random.Range(-5, 5) * Vector3.right + position;
                enemy.SetActive(true);
                return enemy;
            }
        }

        GameObject newEnemy = Instantiate(enemyPref, transform);
        newEnemy.transform.position = enemyPos[Random.Range(0, enemyPos.Count)].position;//Random.Range(-5, 5) * Vector3.right + position;
        enemies.Add(newEnemy);
        return newEnemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }
    public void ReturnAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }
}
