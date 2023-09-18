using System.Runtime.InteropServices;

namespace TabletFriend.TabletMode
{
    public class TabletModeDetector
    {
        private static readonly int SM_CONVERTIBLESLATEMODE = 0x2003;
        private static readonly int SM_TABLETPC = 0x56;

        private static bool _isTabletPC = false;

        public static bool SupportsTabletMode => _isTabletPC;

        public static bool IsTabletMode => QueryTabletMode();

        static TabletModeDetector()
        {
            _isTabletPC = GetSystemMetrics(SM_TABLETPC) != 0;
        }


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "GetSystemMetrics")]
        private static extern int GetSystemMetrics(int nIndex);


        private static bool QueryTabletMode()
        {
            int state = GetSystemMetrics(SM_CONVERTIBLESLATEMODE);
            return state == 0 && _isTabletPC;
        }
    }
}
