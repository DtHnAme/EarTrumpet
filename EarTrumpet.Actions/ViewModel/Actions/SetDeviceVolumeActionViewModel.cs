﻿using EarTrumpet_Actions.DataModel.Actions;
using EarTrumpet_Actions.DataModel.Enum;

namespace EarTrumpet_Actions.ViewModel.Actions
{
    class SetDeviceVolumeActionViewModel : PartViewModel
    {
        public OptionViewModel Option { get; }
        public DeviceViewModel Device { get; }
        public VolumeViewModel Volume { get; }

        private SetDeviceVolumeAction _action;

        public SetDeviceVolumeActionViewModel(SetDeviceVolumeAction action) : base(action)
        {
            _action = action;
            Option = new OptionViewModel(action);
            Option.PropertyChanged += (_, __) => UpdateDescription();
            Device = new DeviceViewModel(action, DataModel.Device.DeviceListKind.Recording | DataModel.Device.DeviceListKind.DefaultPlayback);
            Device.PropertyChanged += (_, __) => UpdateDescription();
            Volume = new VolumeViewModel(action);
            Volume.PropertyChanged += (_, __) => UpdateDescription();
        }
    }
}
