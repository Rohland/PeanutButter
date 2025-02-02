using NUnit.Framework;
using static NExpect.Expectations;
using NExpect;

namespace PeanutButter.Utils.Tests
{
    [TestFixture]
    public class TestFind
    {
        [Test]
        public void ShouldBeAbleToFindNotePad()
        {
            if (!Platform.IsWindows)
            {
                Assert.Ignore("Looking for notepad.exe on !windows is unlikely to succeed");
                return;
            }

            // Arrange
            var search = "notepad";
            var expected = "C:\\Windows\\system32\\notepad.EXE"; // capitalization of extension is thanks to PATHEXT
            // Act
            var result = Find.InPath(search);
            // Assert
            Expect(result).To.Equal(expected);
        }
    }
}