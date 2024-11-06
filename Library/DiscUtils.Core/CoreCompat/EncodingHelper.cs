#if !NET45 && !NET472 && !NET48
using System.Text;
#endif

namespace DiscUtils.CoreCompat
{
    internal static class EncodingHelper
    {
        private static bool _registered;

        public static void RegisterEncodings()
        {
            if (_registered)
                return;

            _registered = true;

#if !NET45 && !NET472 && !NET48
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }
    }
}