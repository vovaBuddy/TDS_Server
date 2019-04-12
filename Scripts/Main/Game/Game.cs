using UnityEngine;
using Core.MessageBus;
using System.Collections;
using Core.MessageBus.MessageDataTemplates;

namespace Main.Game
{
    public class Game : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        { 
            Application.targetFrameRate = 120;
            QualitySettings.vSyncCount = 0;
        }
    }
}