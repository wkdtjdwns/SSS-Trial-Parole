using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Models
{
    #region Player

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        //¹Î°¨µµ
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXinverted;
        public bool ViewYinverted;

        [Header("Movement")]
        public float walkingForwardSpeed;
        public float walkingBackwardSpeed;
        public float WalkingStrafeSpeed;
    }
    #endregion
}