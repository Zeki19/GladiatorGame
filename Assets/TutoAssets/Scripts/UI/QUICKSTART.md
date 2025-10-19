# Tutorial Quest Panel - Quick Start Guide

## Fastest Setup (Using Editor Tool)

### Option 1: Automated Setup (Recommended)

1. **Open the Setup Tool**:
   - In Unity, go to: `Tools Å® Tutorial Å® Setup Quest Panel UI`

2. **Create Components**:
   - Click "Create Quest Entry Prefab" (creates prefab automatically)
   - Click "Create Quest Panel in Scene" (creates panel in your Canvas)

3. **Assign Prefab**:
   - Select the "TutorialQuestPanel" GameObject in Hierarchy
   - In Inspector, find the `TutorialQuestPanelUI` component
   - Drag the "TutorialQuestEntry" prefab from `Assets/TutoAssets/Prefabs/UI/` to the "Quest Entry Prefab" field

4. **Done!** Press Play to test.

---

## Manual Setup (If You Prefer Control)

Follow the detailed instructions in `README_QuestPanelSetup.md`

---

## How It Works

### Automatic Integration
The system automatically connects to your existing `TutorialManager` through events:

```
TutorialManager Å® Events Å® TutorialQuestPanelUI
```

No code changes required in TutorialManager!

### Event Flow
1. **Tutorial Starts** Å® Panel appears (if not in training mode)
2. **Mission Starts** Å® New quest entry spawns with mission name and description
3. **Mission Completes** Å® Old entry fades out
4. **Next Mission Starts** Å® New entry fades in
5. **Tutorial Completes** Å® Shows completion message or hides panel

### Data Source
The panel reads directly from `TutorialMission` ScriptableObjects:
- `missionName` Å® Quest Label/Title
- `missionDescription` Å® Quest Goal/Description

---

## Testing

### In-Editor Test
1. Select `TutorialQuestPanel` in Hierarchy
2. Right-click the `TutorialQuestPanelUI` component
3. Choose "Test Show Completion" or "Test Restart"

### In Play Mode
1. Press Play
2. Panel should appear when tutorial starts
3. Complete missions to see dynamic updates
4. Panel updates automatically as you progress

---

## Customization Quick Tips

### Change Panel Position
- Select `TutorialQuestPanel`
- Modify `anchoredPosition` in RectTransform
- Default is Top-Left (30, -30)

### Change Colors
- **Panel Background**: Edit Image component color on TutorialQuestPanel
- **Text Colors**: Edit the prefab's TextMeshProUGUI components
- **Completion Message**: Edit CompletionMessage TextMeshProUGUI color

### Change Fade Speed
- Select the TutorialQuestEntry prefab
- In `TutorialQuestEntryUI` component:
  - `fadeInDuration` - Speed of quest appearing
  - `fadeOutDuration` - Speed of quest disappearing

### Hide Panel After Completion
- Select TutorialQuestPanel
- Check "Hide On Completion" in `TutorialQuestPanelUI`

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Panel doesn't appear | Check TutorialManager is in scene and not in Training Mode |
| No quest text showing | Verify Quest Entry Prefab is assigned |
| Text is cut off | Increase panel size or enable word wrapping on text |
| Prefab missing references | Re-create using the editor tool |

---

## Advanced Features (Optional)

### Add Icons to Quests
1. Edit TutorialQuestEntry prefab
2. Add Image component for icon
3. Modify `TutorialQuestEntryUI.cs` to accept icon sprite
4. Pass icon from TutorialMission

### Add Progress Bars
1. Add Slider to TutorialQuestEntry prefab
2. Create progress tracking in TutorialMission
3. Update slider via events

### Multiple Quests Display
1. Modify `TutorialQuestPanelUI` to track list of entries
2. Don't destroy previous entries, add new ones
3. Mark completed quests with strikethrough or checkmark

---

## File Locations

- **Scripts**: `Assets/TutoAssets/Scripts/UI/`
  - `TutorialQuestPanelUI.cs` - Main panel manager
  - `TutorialQuestEntryUI.cs` - Individual quest entry

- **Editor Tools**: `Assets/TutoAssets/Scripts/Editor/`
  - `TutorialQuestPanelSetup.cs` - Setup wizard

- **Prefabs**: `Assets/TutoAssets/Prefabs/UI/`
  - `TutorialQuestEntry.prefab` - Quest entry template

- **Documentation**: `Assets/TutoAssets/Scripts/UI/`
  - `README_QuestPanelSetup.md` - Detailed setup guide
  - `QUICKSTART.md` - This file

---

## Support

If you encounter issues:
1. Check console for error messages
2. Verify all references are assigned in Inspector
3. Ensure TextMeshPro is imported (Window Å® TextMeshPro Å® Import TMP Essential Resources)
4. Review the detailed README for step-by-step instructions

---

**That's it! Enjoy your dynamic tutorial quest panel!** ??
