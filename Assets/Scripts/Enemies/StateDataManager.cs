using System.Collections.Generic;

public class StateDataManager
{
    private Dictionary<EnemyStates, IStateData> _stateData = new Dictionary<EnemyStates, IStateData>();

    public void SetStateData(EnemyStates state, IStateData data)
    {
        _stateData[state] = data;
    }

    public T GetStateData<T>(EnemyStates state) where T : class, IStateData
    {
        return _stateData.TryGetValue(state, out var data) ? data as T : null;
    }

    public void ClearStateData(EnemyStates state)
    {
        _stateData.Remove(state);
    }
}