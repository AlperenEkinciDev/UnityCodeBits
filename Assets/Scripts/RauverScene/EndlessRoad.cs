using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRoad : MonoBehaviour
{
    [Header("EndlessRoadSettings")]
    [SerializeField] Transform[] roadPieces;
    [SerializeField] Transform startPos, endPos;

    private int[] dotArray;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        PlaceOutBoubdPieces();
    }

    void Setup()
    {
        dotArray = new int[roadPieces.Length];
        
        for (int i = 0; i < dotArray.Length; i++)
        {
            dotArray[i] = (int) Mathf.Sign(Vector3.Dot(roadPieces[i].forward, (roadPieces[i].position - endPos.position).normalized));
        }
    }

    void PlaceOutBoubdPieces()
    {
        for (int i = 0; i < roadPieces.Length; i++)
        {
            Transform tempRoadPiece = roadPieces[i];

            if ((int)Mathf.Sign(Vector3.Dot(roadPieces[i].forward, (roadPieces[i].position - endPos.position).normalized)) != dotArray[i])
            {
                tempRoadPiece.position = startPos.position;
            }
        }
    }
}
