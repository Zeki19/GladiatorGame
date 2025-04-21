using UnityEngine;

public class DSIdle<T> : DummyBase<T>
{
    private T _transitionToSearch;
    
    public DSIdle(T transitionToSearch)
    {
        _transitionToSearch = transitionToSearch;
    }
    
}
