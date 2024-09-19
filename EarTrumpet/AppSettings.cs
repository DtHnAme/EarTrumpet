using EarTrumpet.DataModel.Storage;
using EarTrumpet.Interop.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using static EarTrumpet.Interop.User32;

namespace EarTrumpet
{
    public class AppSettings
    {
        public event EventHandler<bool> UseLegacyIconChanged;
        public event Action FlyoutHotkeyTyped;
        public event Action MixerHotkeyTyped;
        public event Action SettingsHotkeyTyped;
        public event Action AbsoluteVolumeUpHotkeyTyped;
        public event Action AbsoluteVolumeDownHotkeyTyped;

        private ISettingsBag _settings = StorageFactory.GetSettings();

        public void RegisterHotkeys()
        {
            HotkeyManager.Current.Register(FlyoutHotkey);
            HotkeyManager.Current.Register(MixerHotkey);
            HotkeyManager.Current.Register(SettingsHotkey);
            HotkeyManager.Current.Register(AbsoluteVolumeUpHotkey);
            HotkeyManager.Current.Register(AbsoluteVolumeDownHotkey);

            HotkeyManager.Current.KeyPressed += (hotkey) =>
            {
                if (hotkey.Equals(FlyoutHotkey))
                {
                    Trace.WriteLine("AppSettings FlyoutHotkeyTyped");
                    FlyoutHotkeyTyped?.Invoke();
                }
                else if (hotkey.Equals(SettingsHotkey))
                {
                    Trace.WriteLine("AppSettings SettingsHotkeyTyped");
                    SettingsHotkeyTyped?.Invoke();
                }
                else if (hotkey.Equals(MixerHotkey))
                {
                    Trace.WriteLine("AppSettings MixerHotkeyTyped");
                    MixerHotkeyTyped?.Invoke();
                }
                else if (hotkey.Equals(AbsoluteVolumeUpHotkey))
                {
                    Trace.WriteLine("AppSettings AbsoluteVolumeUpHotkeyTyped");
                    AbsoluteVolumeUpHotkeyTyped?.Invoke();
                }
                else if (hotkey.Equals(AbsoluteVolumeDownHotkey))
                {
                    Trace.WriteLine("AppSettings AbsoluteVolumeDownHotkeyTyped");
                    AbsoluteVolumeDownHotkeyTyped?.Invoke();
                }
            };
        }

        public HotkeyData FlyoutHotkey
        {
            get => _settings.Get("Hotkey", new HotkeyData { });
            set
            {
                HotkeyManager.Current.Unregister(FlyoutHotkey);
                _settings.Set("Hotkey", value);
                HotkeyManager.Current.Register(FlyoutHotkey);
            }
        }

        public HotkeyData MixerHotkey
        {
            get => _settings.Get("MixerHotkey", new HotkeyData { });
            set
            {
                HotkeyManager.Current.Unregister(MixerHotkey);
                _settings.Set("MixerHotkey", value);
                HotkeyManager.Current.Register(MixerHotkey);
            }
        }

        public HotkeyData SettingsHotkey
        {
            get => _settings.Get("SettingsHotkey", new HotkeyData { });
            set
            {
                HotkeyManager.Current.Unregister(SettingsHotkey);
                _settings.Set("SettingsHotkey", value);
                HotkeyManager.Current.Register(SettingsHotkey);
            }
        }

        public HotkeyData AbsoluteVolumeUpHotkey
        {
            get => _settings.Get("AbsoluteVolumeUpHotkey", new HotkeyData { });
            set
            {
                HotkeyManager.Current.Unregister(AbsoluteVolumeUpHotkey);
                _settings.Set("AbsoluteVolumeUpHotkey", value);
                HotkeyManager.Current.Register(AbsoluteVolumeUpHotkey);
            }
        }

        public HotkeyData AbsoluteVolumeDownHotkey
        {
            get => _settings.Get("AbsoluteVolumeDownHotkey", new HotkeyData { });
            set
            {
                HotkeyManager.Current.Unregister(AbsoluteVolumeDownHotkey);
                _settings.Set("AbsoluteVolumeDownHotkey", value);
                HotkeyManager.Current.Register(AbsoluteVolumeDownHotkey);
            }
        }

        public bool UseLegacyIcon
        {
            get
            {
                // Note: Legacy compat, we used to write string bools.
                var ret = _settings.Get("UseLegacyIcon", "False");
                bool.TryParse(ret, out bool isUseLegacyIcon);
                return isUseLegacyIcon;
            }
            set
            {
                _settings.Set("UseLegacyIcon", value.ToString());
                UseLegacyIconChanged?.Invoke(null, UseLegacyIcon);
            }
        }

        public bool IsExpanded
        {
            get => _settings.Get("IsExpanded", false);
            set => _settings.Set("IsExpanded", value);
        }

        public bool UseScrollWheelInTray
        {
            get => _settings.Get("UseScrollWheelInTray", true);
            set => _settings.Set("UseScrollWheelInTray", value);
        }

        public bool UseGlobalMouseWheelHook
        {
            get => _settings.Get("UseGlobalMouseWheelHook", false);
            set => _settings.Set("UseGlobalMouseWheelHook", value);
        }

        public bool HasShownFirstRun
        {
            get => _settings.HasKey("hasShownFirstRun");
            set => _settings.Set("hasShownFirstRun", value);
        }

        public bool UseLogarithmicVolume
        {
            get => _settings.Get("UseLogarithmicVolume", false);
            set => _settings.Set("UseLogarithmicVolume", value);
        }

        public WINDOWPLACEMENT? FullMixerWindowPlacement
        {
            get => _settings.Get("FullMixerWindowPlacement", default(WINDOWPLACEMENT?));
            set => _settings.Set("FullMixerWindowPlacement", value);
        }

        public WINDOWPLACEMENT? SettingsWindowPlacement
        {
            get => _settings.Get("SettingsWindowPlacement", default(WINDOWPLACEMENT?));
            set => _settings.Set("SettingsWindowPlacement", value);
        }
    }
}