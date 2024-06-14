using RoadToAAA.ProjectClock.Managers;
using System;
using System.Linq;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "ComboAsset", menuName = "ConfigurationAssets/ComboAsset")]
    public class ComboAsset : ScriptableObject
    {
        public ECheckResult[] ProgressConditions;
        public ECheckResult[] FailConditions;
        public ComboState[] ComboStates;

        public ComboResult GetComboState(ECheckResult checkResult, int currentState, int currentNumberOfSuccessConditions)
        {
            Debug.Assert(currentState >= 0 && currentState < ComboStates.Length, "CurrentState does not exists!");
            Debug.Assert(currentNumberOfSuccessConditions < ComboStates[currentState].NumberOfSuccessConditionsToChangeAsset, "CurrentNumberOfSuccess in this state already exceeded the limit!");
            Debug.Assert(currentNumberOfSuccessConditions >= 0, "CurrentNumberOfSuccess is not valid!");

            ComboResult result = new ComboResult();

            if (ProgressConditions.Contains(checkResult))
            {
                if (currentState == ComboStates.Length - 1)
                {
                    result.StateIndex = currentState;
                    result.Type = EComboResult.Saturated;
                    return result;
                }
                else if (currentNumberOfSuccessConditions >= ComboStates[currentState].NumberOfSuccessConditionsToChangeAsset - 1)
                {
                    currentState++;

                    result.StateIndex = currentState;
                    result.Type = EComboResult.NewState;
                    return result;
                }
                else
                {
                    result.StateIndex = currentState;
                    result.Type = EComboResult.Progress;
                    return result;
                }
            }
            
            if (FailConditions.Contains(checkResult)) 
            {
                currentState = 0;

                result.StateIndex = currentState;
                result.Type = EComboResult.Fail;
                return result;
            }

            result.StateIndex = currentState;
            result.Type = EComboResult.None;
            return result;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            Debug.Assert(IsValid(), "Combo asset is not valid!");
        }
#endif
        private bool IsValid()
        {
            if (ProgressConditions.Length <= 0) return false;
            if (FailConditions.Length <= 0) return false;
            if (ComboStates.Length <= 0) return false;
            if (ProgressConditions.Intersect(FailConditions).Any()) return false;
            if (ComboStates[ComboStates.Length - 1].NumberOfSuccessConditionsToChangeAsset > 1) return false;

            for (int i = 0; i < ComboStates.Length; i++)
            {
                ComboState currentState = ComboStates[i];

                if (currentState.Score <= 0) return false;

                if (currentState.Message == "") return false;

                if (currentState.NumberOfSuccessConditionsToChangeAsset <= 0) return false;

                if (currentState.MessageColor.a != 1.0f) return false;
            }

            for (int i = 1; i < ComboStates.Length; i++)
            {
                ComboState preState = ComboStates[i - 1];
                ComboState currentState = ComboStates[i];

                if (preState.Score > currentState.Score)
                {
                    return false;
                }
            }

            return true;
        }
    }

    [Serializable]
    public class ComboState
    {
        public int Score = 1;
        public string Message;
        public Color MessageColor;
        public int NumberOfSuccessConditionsToChangeAsset = 1;
    }

    /*
     *  None = Error state, this should never happen
     *  Progress = State didn't changed but success condition was true (i.e. +1 to successConditionCounter to change State)
     *  New State = State changed due to a success condition
     *  Saturated = State didn't changed but success condition was true because it is the last state
     *  Fail = State went back to 0 due to a fail
     */
    public enum EComboResult
    {
        None,
        Progress,
        NewState,
        Saturated,
        Fail
    }

    public struct ComboResult
    {
        public int StateIndex;
        public EComboResult Type;

        public override string ToString()
        {
            return string.Format("State: {0}, Type: {1}", StateIndex, Type);
        }
    }
}
