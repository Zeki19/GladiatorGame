using System;
using System.Collections.Generic;
using Core.Status;
using UnityEngine;

[Serializable]
public class EntityStatusManager<TSpecificStatus> where TSpecificStatus : Enum
{
    private Dictionary<CommonStatus, bool> _commonStatus = new();
    private Dictionary<TSpecificStatus, bool> _specificStatus = new();
    [SerializeField] private List<CommonPair> InitialCommonStatuses;
    [SerializeField] private List<SpesificStatus<TSpecificStatus>> InitialSpesificStatuses;

    public void SetUpStatus()
    {
        // Initialize common Status
        foreach (CommonStatus Status in Enum.GetValues(typeof(CommonStatus)))
            _commonStatus[Status] = false;

        // Initialize specific Status
        foreach (TSpecificStatus Status in Enum.GetValues(typeof(TSpecificStatus)))
            _specificStatus[Status] = false;

        SetUpStartingValues();
    }

    private void SetUpStartingValues()
    {
        if (InitialCommonStatuses != null)
            foreach (var status in InitialCommonStatuses)
                _commonStatus[status.status] = status.value;

        if (InitialSpesificStatuses != null)
            foreach (var status in InitialSpesificStatuses)
                _specificStatus[status.status] = status.value;
    }
    
    private void SetStatusGeneric<T>(Dictionary<T, bool> statusDictionary, T status, bool value) where T : Enum
    {
        statusDictionary[status] = value;
    }

    public void SetStatus(CommonStatus status, bool value)
    {
        SetStatusGeneric(_commonStatus, status, value);
    }

    public void SetStatus(TSpecificStatus status, bool value)
    {
        SetStatusGeneric(_specificStatus, status, value);
    }
    private bool GetStatusGeneric<T>(Dictionary<T, bool> statusDictionary, T status) where T : Enum
    {
        return statusDictionary.TryGetValue(status, out bool value) && value;
    }

    public bool GetStatus(CommonStatus status)
    {
        return GetStatusGeneric(_commonStatus, status);
    }

    public bool GetStatus(TSpecificStatus status)
    {
        return GetStatusGeneric(_specificStatus, status);
    }
}


namespace Core.Status
{
    [Serializable]
    public class CommonPair
    {
        public CommonStatus status;
        public bool value;
    }
    [Serializable]
    public class SpesificStatus<T>
    {
        public T status;
        public bool value;
    }
    
    // Common Status shared by all entities
    public enum CommonStatus
    {
        IsAlive,
        IsStunned,
        IsInvulnerable,
        IsMoving
    }
    // Specific Status for the Player
    public enum PlayerStatus
    {
        IsAlive,
        IsStunned,
        IsInvulnerable,
        IsMoving
    }
    // Specific Status for the gaius
    public enum GaiusStatus
    {
        IsAlive,
        IsStunned,
        IsInvulnerable,
        IsMoving
    }
}

