﻿using EarTrumpet_Actions.DataModel.Actions;
using EarTrumpet_Actions.DataModel.Enum;

namespace EarTrumpet_Actions.ViewModel.Actions
{
    class SetAppVolumeActionViewModel : PartViewModel
    {
        public OptionViewModel Option { get; }
        public DeviceViewModel Device { get; }
        public AppViewModel App { get; }
        public VolumeViewModel Volume { get; }

        private SetAppVolumeAction _action;

        public SetAppVolumeActionViewModel(SetAppVolumeAction action) : base(action)
        {
            _action = action;

            Option = new OptionViewModel(action);
            Option.PropertyChanged += (_, __) => UpdateDescription();
            App = new AppViewModel(action, DataModel.App.AppKind.EveryApp | DataModel.App.AppKind.ForegroundApp);
            App.PropertyChanged += (_, __) => UpdateDescription();
            Device = new DeviceViewModel(action, DataModel.Device.DeviceListKind.DefaultPlayback);
            Device.PropertyChanged += (_, __) => UpdateDescription();
            Volume = new VolumeViewModel(action);
            Volume.PropertyChanged += (_, __) => UpdateDescription();
        }
    }
}
