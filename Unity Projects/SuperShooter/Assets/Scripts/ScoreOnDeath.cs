using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnDeath : MonoBehaviour
{
    [SerializeField]
    private int amount;
    private Life life;

    private void Awake()
    {
        life = GetComponent<Life>();
        life.onDeath.AddListener(GivePoints);  
    }

    private void GivePoints()
    {
        ScoreManager.instance.amount += amount;
    }

    private void OnDestroy()
    {
        life.onDeath.RemoveListener(GivePoints);
    }
}
