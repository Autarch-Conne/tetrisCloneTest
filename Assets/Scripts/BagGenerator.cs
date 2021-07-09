using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagGenerator : MonoBehaviour
{
    public List<PieceType> bag = new List<PieceType>();

    // Update is called once per frame
public PieceType DrawPiece()
 {
     if (bag.Count == 0)
     {
         foreach(PieceType i in System.Enum.GetValues(typeof(PieceType))){
             bag.Add(i);
         }
     }
     int index = Random.Range(0, bag.Count);
     PieceType p = bag[index];
     bag.RemoveAt(index);
     return p;
 }
}
