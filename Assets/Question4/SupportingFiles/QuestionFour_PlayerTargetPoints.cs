using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.flobelle
{

    [CreateAssetMenu(menuName = "QuestionFour/Target Positions")]
    public class QuestionFour_PlayerTargetPoints : ScriptableObject
    {
        public List<Vector3> Positions = new List<Vector3>();
    }

}