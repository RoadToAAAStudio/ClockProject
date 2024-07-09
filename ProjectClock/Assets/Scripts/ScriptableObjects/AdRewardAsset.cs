using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "AdRewardAsset", menuName = "ConfigurationAssets/AdRewardAsset")]
    public class AdRewardAsset : ValidableScriptableObject
    {
        public ERewardType RewardType;
        public float Amount = 1.0f;

        public override ScriptableObjectValidateResult CheckValidation()
        {
            ScriptableObjectValidateResult result = new ScriptableObjectValidateResult();
            result.IsValid = true;

            if (Amount <= 0)
            {
                result.IsValid = false;
                result.Message += "Amount should be positive!\n";
            }

            if (result.IsValid)
            {
                result.Message += "Successful!";
            }

            return result;
        }
    }

    public enum ERewardType
    {
        SUM,
        MULTIPLY
    }
}
