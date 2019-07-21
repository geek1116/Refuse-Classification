using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager
{
    public LinkedList<GameObject> garbages = new LinkedList<GameObject>();

    public void ChangeGarbagesSpeed(float _speed)
    {
        foreach(GameObject go in garbages)
        {
            go.GetComponent<Garbage>().SetSpeed(_speed);
        }
    }

    /// <summary>
    /// Note: new garbage will be added as last node in the list
    /// </summary>
    /// <param name="garbage"></param>
    public void AddGarbage(GameObject garbage)
    {
        garbages.AddLast(garbage);
    }

    public void RemoveGarbage(GameObject garbage)
    {
        garbages.Remove(garbage);
        garbage.GetComponent<Garbage>().Eliminate();
    }

    public void RemindLastUnmatchGarbage(List<int> carType)
    {
        GameObject unmatchGarbage = FindLastNotMatchGarbage(carType);
        if(unmatchGarbage != null)
        {
            garbages.Remove(unmatchGarbage);
            unmatchGarbage.GetComponent<Garbage>().Remind();
        }

    }

    public void EliminateLastUnmatchGarbage(List<int> carType)
    {
        GameObject unmatchGarbage = FindLastNotMatchGarbage(carType);
        if (unmatchGarbage != null)
        {
            garbages.Remove(unmatchGarbage);
            unmatchGarbage.GetComponent<Garbage>().Eliminate();
        }
    }

    private GameObject FindLastNotMatchGarbage(List<int> carType)
    {
        GameObject unmatchGarbage = null;
        // forward iteration
        foreach (GameObject go in garbages)
        {
            bool isMatch = false;
            int garbageType = go.GetComponent<Garbage>().type;
            foreach (int type in carType)
            {
                if (garbageType == type)
                {
                    isMatch = true;
                    break;
                }
            }
            if (!isMatch)
            {
                unmatchGarbage = go;
                break;
            }
        }
        return unmatchGarbage;
    }
}
