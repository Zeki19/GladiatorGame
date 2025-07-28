public interface IStatesData
{
    public void SetStateData(EnemyStates state, IStateData data);

    public T GetStateData<T>(EnemyStates state) where T : class, IStateData;

}