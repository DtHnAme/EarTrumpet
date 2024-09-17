using EarTrumpet.Extensions;
using EarTrumpet.Interop;
using System;
using System.Diagnostics;

namespace EarTrumpet.DataModel.AppInformation.Internal
{
    class SystemSoundsAppInfo : IAppInfo
    {
        public event Action<IAppInfo> Stopped { add { } remove { } }
        public uint BackgroundColor => 0x000000;
        public string ExeName => "*SystemSounds";
        public string DisplayName => null;
        public string PackageInstallPath => "System.SystemSoundsSession";
        public bool IsDesktopApp => true;
        public string SmallLogoPath => @"%windir%\system32\audiosrv.dll,203";
    }
}
