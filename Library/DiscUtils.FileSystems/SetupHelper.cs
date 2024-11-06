using DiscUtils.CoreCompat;
using DiscUtils.HfsPlus;

namespace DiscUtils.FileSystems
{
public static class SetupHelper
{
    public static void SetupFileSystems()
    {
        Setup.SetupHelper.RegisterAssembly(ReflectionHelper.GetAssembly(typeof(HfsPlusFileSystem)));
    }
}
}