using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperMarketGame
{
    public class FramRateManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}