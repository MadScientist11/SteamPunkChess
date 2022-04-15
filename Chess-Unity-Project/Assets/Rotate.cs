using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class Rotate : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.Rotate(transform.up, 180f, Space.World);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
