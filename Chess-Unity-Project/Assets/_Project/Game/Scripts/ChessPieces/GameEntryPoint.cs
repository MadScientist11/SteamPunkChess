using System;
using System.Collections;
using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    private void Awake()
    {
        ChessBoard board = BoardFactory.Create();

    }
}
