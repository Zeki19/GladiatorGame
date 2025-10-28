using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private readonly List<Resolution> _filteredResolutions = new List<Resolution>();

    private void Start()
    {
        SetupResolutions();
        if (fullscreenToggle) fullscreenToggle.isOn = Screen.fullScreen;
    }
    private void SetupResolutions()
    {
        var all = Screen.resolutions;
        var current = Screen.currentResolution;
        
        _filteredResolutions.Clear();
        resolutionDropdown.ClearOptions();
        
        var seen = new HashSet<(int w, int h)>();
        foreach (var r in all)
        {
            if (!SameHz(r, current)) continue;
            var key = (r.width, r.height);
            if (seen.Add(key)) _filteredResolutions.Add(r);
        }
        
        if (_filteredResolutions.Count == 0)
        {
            _filteredResolutions.Add(current);
        }
        
        var options = new List<string>(_filteredResolutions.Count);
        int currentIdx = 0;
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            var r = _filteredResolutions[i];
            options.Add($"{r.width} x {r.height}");
            if (r.width == current.width && r.height == current.height)
                currentIdx = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIdx;
        resolutionDropdown.RefreshShownValue();
    }
    
    public void SetResolution(int dropdownIndex)
    {
        if (dropdownIndex < 0 || dropdownIndex >= _filteredResolutions.Count) return;
        
        var r = _filteredResolutions[dropdownIndex];
        
        var mode = Screen.fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        
        Screen.SetResolution(r.width, r.height, mode, r.refreshRateRatio);
        
    }
    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreenMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    #region Utils

    static float GetHz(in Resolution r)
    {
        return (float)r.refreshRateRatio.value;
    }
    static bool SameHz(in Resolution a, in Resolution b)
    {
        return Mathf.Abs(GetHz(a) - GetHz(b)) < 0.5f;
    }

    #endregion
}
