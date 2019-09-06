using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeapOptim<T> where T: IHeapItem<T>
{

    T[] items;
    int currentItemCount;

    public HeapOptim(int maxHeapSize) {
        //Debug.Log("maxHeapSize= "+maxHeapSize);
        items = new T[maxHeapSize];
    }
        
    public void HeapAdd(T n) {
        n.NodeHeapIndex = currentItemCount;
        //Debug.Log("currentItemCOunt in HeapAdd= " + currentItemCount);
        items[currentItemCount] = n;
        SortUp(n);
        currentItemCount++;
    }

    public T RemoveFirst() {
        T firstItem = items[0];
        //Debug.Log("currentItemCOunt in RemoveFirst 1/2 = " + currentItemCount);
        currentItemCount--;
        //Debug.Log("currentItemCOunt in RemoveFirst 2/2 = " + currentItemCount);
        items[0] = items[currentItemCount];
        items[0].NodeHeapIndex = 0;
        SortDown(items[0]);
        //Debug.Log("First item: "+firstItem);
        return firstItem;
    }

    public void UpdateHeapNode (T n) {
        SortUp(n);
    }
    public int HeapCount {
        get {
            return currentItemCount;
        }
    }
    public bool Contains(T n) {
        return Equals(items[n.NodeHeapIndex], n);
    }
        
    void SortDown(T n) {
        while (true) {
            int childIndexLeft = n.NodeHeapIndex * 2 + 1;
            int childIndexRight = n.NodeHeapIndex * 2 + 2;
            int swapIndex = 0;

            if(childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if(childIndexRight< currentItemCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

                if (n.CompareTo(items[swapIndex]) < 0) {
                    Swap(n, items[swapIndex]);
                }
                else {
                    return;
                }
            }
            else {
                return;
            }
        }
    }

    void SortUp(T n) {
        int parentIndex = (n.NodeHeapIndex - 1) / 2;

        while (true) {
            T parentItem = items[parentIndex];
            if (n.CompareTo(parentItem) > 0) {
                Swap(n, parentItem);
            }
            else { break; }

            parentIndex = (n.NodeHeapIndex - 1) / 2;
        }
    }


    void Swap(T itemA, T itemB) {
        items[itemA.NodeHeapIndex] = itemB;
        items[itemB.NodeHeapIndex] = itemA;

        int itemAIndex = itemA.NodeHeapIndex;
        itemA.NodeHeapIndex = itemB.NodeHeapIndex;
        itemB.NodeHeapIndex = itemAIndex;

    }

}

public interface IHeapItem<T>: IComparable<T> {
    int NodeHeapIndex {
        get;
        set;
    }

}
