using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager
{
    public LinkedList<GameObject> garbages = new LinkedList<GameObject>();

    public void ChangeGarbagesSpeed(float _speed)
    {
        foreach (GameObject go in garbages)
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
        garbage.GetComponent<Garbage>().Destroy();
    }

    private void RemoveGarbage(LinkedListNode<GameObject> node)
    {
        garbages.Remove(node);
        node.Value.GetComponent<Garbage>().Destroy();
    }

    public void ClearGarbages()
    {
        foreach(GameObject garbage in garbages)
        {
            garbage.GetComponent<Garbage>().Destroy();
        }
        garbages.Clear();
    }

    public bool IsEmpty()
    {
        return garbages.Count < 1;
    }

    public void ChangeGarbageTypeRandomly(GameObject garbage)
    {
        garbage.GetComponent<Garbage>().Reset(GameData.config.GetGarbageData(Random.Range(1, GameData.config.GetGarbageDataCount())));
    }

    public void TransferNodeTo(Vector2 target)
    {
        // TODO: reference a arrPoint in Scene, and find out the node before this point.
    }

    public void RemindLastUnmatchGarbage(List<int> carType)
    {
        LinkedListNode<GameObject> unmatchNode = FindLastNotMatchNode(carType);
        if(unmatchNode != null)
        {
            unmatchNode.Value.GetComponent<Garbage>().Remind();
        }
    }

    public void EliminateLastUnmatchGarbage(List<int> carType)
    {
        LinkedListNode<GameObject> unmatchNode = FindLastNotMatchNode(carType);
        if (unmatchNode != null)
        {
            garbages.Remove(unmatchNode);
            unmatchNode.Value.GetComponent<Garbage>().Destroy();
        }
    }

    public void EliminateBothSizeGarbage(GameObject garbage)
    {
        LinkedListNode<GameObject> node = garbages.Find(garbage);
        if(node == null) { Debug.LogError("This garbage does not added to scene."); }
        LinkedListNode<GameObject> next = node.Next;
        LinkedListNode<GameObject> previous = node.Previous;

        RemoveGarbage(node);
        RemoveGarbage(next);
        RemoveGarbage(previous);
    }

    public void EliminateLastPerniciousGarbage()
    {
        LinkedListNode<GameObject> perniciousNode = FindLastSpecificTypeNode(GarbageData.GarbageType.Pernicious);
        RemoveGarbage(perniciousNode);
    }

    public void CopyBeforeGarbage(GameObject _garbage)
    {
        LinkedListNode<GameObject> node = garbages.Find(_garbage);
        if (node == null) { Debug.LogError("This garbage does not added to scene."); }
        LinkedListNode<GameObject> beforeNode = node.Previous;
        if(beforeNode != null)
        {
            Garbage garbage = node.Value.GetComponent<Garbage>();
            garbage.Reset(beforeNode.Value.GetComponent<Garbage>().garbageData);
            garbage.MoveToLogicPos();
        }
    }

    private LinkedListNode<GameObject> FindLastNotMatchNode(List<int> carType)
    {
        LinkedListNode<GameObject> unmatchNode = null;
        // forward iteration
        LinkedListNode<GameObject> node = garbages.First;
        while(node != null)
        {
            bool isMatch = false;
            int garbageType = node.Value.GetComponent<Garbage>().type;
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
                unmatchNode = node;
                break;
            }
            node = node.Next;
        }
        return unmatchNode;
    }

    private LinkedListNode<GameObject> FindLastSpecificTypeNode(GarbageData.GarbageType type)
    {
        LinkedListNode<GameObject> buffNode = null;

        // forward iteration
        LinkedListNode<GameObject> node = garbages.First;
        while(node != null)
        {
            if(node.Value.GetComponent<Garbage>().type == (int)type)
            {
                buffNode = node;
                break;
            }
            node = node.Next;
        }
        return buffNode;
    }

    private void MoveNodeForward(LinkedListNode<GameObject> nodeToMove, float distance)
    {
        nodeToMove.Value.GetComponent<Garbage>().MoveForward(distance);
    }

}
