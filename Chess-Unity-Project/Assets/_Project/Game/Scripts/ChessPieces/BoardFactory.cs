using System.Collections;
using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;
using Zenject;

public class BoardFactory : MonoBehaviour
{
    public static DiContainer DIContainer;
  

    public static ChessBoard Create()
    {
        ChessBoard board = DIContainer.Instantiate<ChessBoard>();
        return board;
    }
}
