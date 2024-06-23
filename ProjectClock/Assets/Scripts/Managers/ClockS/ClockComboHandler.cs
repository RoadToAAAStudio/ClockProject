using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Core;

namespace RoadToAAA.ProjectClock.Managers
{
    public class ClockComboHandler
    {
        private ComboAsset _comboAsset;
        private int _currentState = 0;
        private int _currentNumberOFSuccessConditions = 0;

        public ClockComboHandler()
        {
            _comboAsset = ConfigurationManager.Instance.ComboAsset;
        }

        public void Initialize()
        {
            _currentState = 0;
            _currentNumberOFSuccessConditions = 0;  
        }

        public ComboResult HandleCheckResult(ECheckResult checkResult)
        {
            ComboResult result = _comboAsset.GetComboState(checkResult, _currentState, _currentNumberOFSuccessConditions);
            _currentState = result.StateIndex;

            switch (result.Type)
            {
                case EComboResult.Fail:
                case EComboResult.NewState:
                    _currentNumberOFSuccessConditions = 0;
                    break;

                case EComboResult.Progress:
                    _currentNumberOFSuccessConditions++;
                    break;
            }

            return result;
        }

    }
}
