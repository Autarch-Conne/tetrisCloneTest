using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum generationMethods {SevenBag, Random}
public class RandomCoordinator : MonoBehaviour
{
    public generationMethods methodOfSelection = generationMethods.SevenBag;

    private BagGenerator bagGenerator;

    private randomGenerator randomGenerator;

    private void Start() {
        bagGenerator = gameObject.GetComponent<BagGenerator>();
        randomGenerator = gameObject.GetComponent<randomGenerator>();
    }
    public PieceType PieceSelector(){
        if(methodOfSelection == generationMethods.SevenBag){
            return bagGenerator.DrawPiece();
        }
        else{
            return randomGenerator.DrawRandomPiece();
        }
    }
}
