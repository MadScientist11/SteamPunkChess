using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class SettingsPopUpMediator : MonoBehaviour
    {
        [SerializeField] private SettingsPopUp _settingsPopUp;

        [Button, DisableInEditorMode] public void OnValueChanged_ChangeResolutionSize(int index) => _settingsPopUp.ChangeResolutionSize(index);
        
        [Button, DisableInEditorMode] public void OnValueChanged_VsyncToggle(bool value) => _settingsPopUp.ToggleVSync(value);
        
        [Button, DisableInEditorMode] public void OnValueChanged_PostProcessingToggle(bool value) => _settingsPopUp.TogglePostProcessing(value);

        [Button, DisableInEditorMode] public void OnValueChanged_ChangeTextureQuality(int index) => _settingsPopUp.ChangeTextureQuality(index);
        
        [Button, DisableInEditorMode] public void OnValueChanged_MusicToggle(bool value) => _settingsPopUp.ToggleMusic(value);
        
        [Button, DisableInEditorMode] public void OnValueChanged_SoundsToggle(bool value) => _settingsPopUp.ToggleSounds(value);
        
        [Button, DisableInEditorMode] public void OnValueChanged_ChangeLanguage(int index) => _settingsPopUp.ChangeLanguage(index);
        
        [Button, DisableInEditorMode] public void OnClick_Logout() => _settingsPopUp.Logout();
        
    }
}
