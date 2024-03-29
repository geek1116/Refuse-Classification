﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager
{
    public LinkedList<GameObject> garbages = new LinkedList<GameObject>();
    public LinkedList<GameObject> thrownGarbages = new LinkedList<GameObject>();

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

    public void ThrowGarbage(GameObject garbage)
    {
        garbages.Remove(garbage);
        thrownGarbages.AddLast(garbage);
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

        foreach(GameObject garbage in thrownGarbages)
        {
            garbage.GetComponent<Garbage>().Destroy();
        }
        thrownGarbages.Clear();
    }

    public bool IsEmpty()
    {
        return garbages.Count < 1;
    }

    public void ChangeGarbageTypeRandomly(GameObject garbage)
    {
        int code = GameData.config.GetRandomCode();
        garbage.GetComponent<Garbage>().ResetGarbageData(GameData.config.GetGarbageData(code));
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
        if(perniciousNode != null)
        {
            RemoveGarbage(perniciousNode);
        }
    }

    public void EliminateLastRecyclableGarbage()
    {
        LinkedListNode<GameObject> recyclableNode = FindLastSpecificTypeNode(GarbageData.GarbageType.Recyclable);
        if(recyclableNode != null)
        {
            RemoveGarbage(recyclableNode);
        }
    }

    public void CopyBeforeGarbage(GameObject _garbage)
    {
        LinkedListNode<GameObject> node = garbages.Find(_garbage);
        if (node == null) { Debug.LogError("This garbage does not added to scene."); return; }
        LinkedListNode<GameObject> previousNode = node.Previous;
        Garbage garbage = node.Value.GetComponent<Garbage>();
        if(previousNode != null)
        {
            garbage.ResetGarbageData(previousNode.Value.GetComponent<Garbage>().garbageData);
        }
        else
        {
            int code = GameData.config.GetRandomCode();
            garbage.ResetGarbageData(GameData.config.GetGarbageData(code));
        }
        garbage.MoveToLogicPos();
    }

    public void PortalTransfer(GameObject garbageToTransfer, Vector2 point, int targetPathIndex, Vector2 targetNodePos)
    {
        LinkedListNode<GameObject> nodeToTransfer = garbages.Find(garbageToTransfer);
        garbages.Remove(nodeToTransfer);

        Garbage garbage = nodeToTransfer.Value.GetComponent<Garbage>();
        garbage.TransferToNewPos(point, targetPathIndex);

        LinkedListNode<GameObject> previousNode = FindPreviousNodeBeforePoint(point, targetPathIndex, targetNodePos);
        if(previousNode == null)
        {
            garbages.AddLast(nodeToTransfer);
        }
        else
        {
            garbages.AddBefore(previousNode, nodeToTransfer);
            PushPreviousNodes(previousNode);
            PushPreviousNodes(nodeToTransfer); // if the previousNode is Far away with next node, we need to confirm the node after transfer node.
        }
    }

    public void SplitMixedGarbage(GameObject mixedGarbage, List<int> splitCodes)
    {
        LinkedListNode<GameObject> mixedNode = garbages.Find(mixedGarbage);
        Garbage mixGarbage = mixedNode.Value.GetComponent<Garbage>();
        foreach (int code in splitCodes)
        {
            GameObject garbage = LevelManager.instance.CreateGarbageAtPos(code, mixedGarbage.transform.position);
            garbage.GetComponent<Garbage>().SetTarget(mixGarbage.GetCurPathIndex());
            garbages.AddBefore(mixedNode, garbage);
        }
        LinkedListNode<GameObject> newNode = mixedNode.Previous;
        PushPreviousNodes(newNode);

        RemoveGarbage(mixedNode);
    }

    private LinkedListNode<GameObject> FindPreviousNodeBeforePoint(Vector2 point, int targetPathIndex, Vector2 targetNodePos)
    {
        float pointToTarget = (targetNodePos - point).magnitude;

        // forward iteration
        LinkedListNode<GameObject> node = garbages.First;
        while (node != null)
        {
            Garbage garbage = node.Value.GetComponent<Garbage>();
            int curIndex = garbage.GetCurPathIndex();
            if(curIndex < targetPathIndex) // node < PathNode
            {
                return node;
            }
            else if(curIndex == targetPathIndex) // node == PathNode
            {
                float nodeToPathNode = (targetNodePos - garbage.GetPos()).magnitude;
                if (nodeToPathNode > pointToTarget)
                {
                    return node;
                }
            }
            node = node.Next;
        }
        return node;
    }

    private void PushPreviousNodes(LinkedListNode<GameObject> node)
    {
        float garbageDistance = LevelManager.instance.GetGarbageDistance();

        LinkedListNode<GameObject> curNode = node;
        LinkedListNode<GameObject> previousNode = curNode.Previous;
        while (previousNode != null)
        {
            Garbage curGarbage = curNode.Value.GetComponent<Garbage>();
            Garbage nextGarbage = previousNode.Value.GetComponent<Garbage>();
            Vector2 offset = (nextGarbage.GetPos() - curGarbage.GetPos());
            float distance = offset.magnitude;
            if (distance < garbageDistance)
            {
                nextGarbage.MoveForward(garbageDistance - distance);
                curNode = curNode.Previous;
                previousNode = previousNode.Previous;
            }
            else
            {
                break;
            }
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

}
