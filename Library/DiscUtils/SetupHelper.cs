using DiscUtils.CoreCompat;
using DiscUtils.HfsPlus;
using DiscUtils.Raw;

namespace DiscUtils.Complete
{
public static class SetupHelper
{
    public static void SetupComplete()
    {
        Setup.SetupHelper.RegisterAssembly(ReflectionHelper.GetAssembly(typeof(Disk)));
        Setup.SetupHelper.RegisterAssembly(ReflectionHelper.GetAssembly(typeof(HfsPlusFileSystem)));
    }
}
}