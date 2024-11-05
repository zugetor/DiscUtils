//
// Copyright (c) 2019, Quamotion bvba
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//

using DiscUtils;
using DiscUtils.HfsPlus;
using DiscUtils.Raw;
using DiscUtils.Setup;
using DiscUtils.Streams;
using System.IO;
using Xunit;

namespace LibraryTests.HfsPlus
{
    public class HfsTest
    {
        private const string iPodPerfPath = @"iPod_Control\Device\Preferences";
        private const string DeviceImagePath = @"D:\iPod\G1-1.0.img";
        private const string DeviceImagePath2 = @"F:\G6-HFS.img";
        static HfsTest()
        {
            SetupHelper.RegisterAssembly(typeof(HfsPlusFileSystem).Assembly);
        }

#if NETCOREAPP
        [Fact]
        public void ReadFileTestHFSW()
        {
            string path = DeviceImagePath;
            using (Stream developerDiskImageStream = File.OpenRead(path))
            using (var disk = new Disk(developerDiskImageStream, Ownership.None))
            {
                // Find the first (and supposedly, only, HFS partition)
                var volumes = VolumeManager.GetPhysicalVolumes(disk);
                Assert.Equal("Apple_MDFW", volumes[0].Partition.TypeAsString);
                Assert.Equal("Apple_HFS", volumes[1].Partition.TypeAsString);
            }
        }

        [Fact]
        public void ReadFileTestHFSP()
        {
            string path = DeviceImagePath2;
            using (Stream developerDiskImageStream = File.OpenRead(path))
            using (var disk = new Disk(developerDiskImageStream, Ownership.None))
            {
                // Find the first (and supposedly, only, HFS partition)
                var volumes = VolumeManager.GetPhysicalVolumes(disk);
                Assert.Equal("Apple_HFS", volumes[0].Partition.TypeAsString);
                foreach (var volume in volumes)
                {
                    if (volume.Partition.TypeAsString != "Apple_HFS")
                    {
                        continue;
                    }
                    var fileSystems = FileSystemManager.DetectFileSystems(volume);

                    var fileSystem = Assert.Single(fileSystems);
                    Assert.Equal("HFS+", fileSystem.Name);

                    using (HfsPlusFileSystem hfs = (HfsPlusFileSystem)fileSystem.Open(volume))
                    {
                        Assert.True(hfs.FileExists(iPodPerfPath));

                        /*using (Stream fileStream = hfs.OpenFile(iPodPerfPath, FileMode.Open, FileAccess.Read))
                        using (MemoryStream copyStream = new MemoryStream())
                        {
                            Assert.NotEqual(0, fileStream.Length);
                            fileStream.Seek(0, SeekOrigin.Begin);
                            copyStream.Seek(0, SeekOrigin.Begin);
                            fileStream.CopyTo(copyStream);
                            Assert.Equal(fileStream.Length, copyStream.Length);

                            copyStream.Seek(0, SeekOrigin.Begin);
                            using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                            {
                                var cont = reader.ReadToEnd();
                            }
                        }*/
                    }
                }
            }
        }
#endif
    }
}
