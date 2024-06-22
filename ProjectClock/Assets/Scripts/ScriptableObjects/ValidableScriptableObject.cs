using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    public abstract class ValidableScriptableObject : ScriptableObject
    {
        public abstract ScriptableObjectValidateResult CheckValidation();
    }

    public struct ScriptableObjectValidateResult
    {
        public bool IsValid;
        public string Message;
    }
}
