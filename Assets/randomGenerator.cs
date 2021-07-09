using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomGenerator : MonoBehaviour
{
    
    public PieceType DrawRandomPiece()
    {
        return  (PieceType)Random.Range(0, 7);   
    }
         
}
