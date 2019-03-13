using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace restlessmedia.Module.File.UnitTest
{
  [TestClass]
  public class MimeTests
  {
    [TestMethod]
    public void TestJpgExtensionWithoutPeriodReturnsJpgMimeType()
    {
      Assert.AreEqual(MimeExtensions.GetMimeType("jpg"), "image/jpeg");
    }

    [TestMethod]
    public void TestJpgExtensionWithPeriodReturnsJpgMimeType()
    {
      Assert.AreEqual(MimeExtensions.GetMimeType(".jpg"), "image/jpeg");
    }

    [TestMethod]
    public void TestNonExistentExtensionReturnsNull()
    {
      MimeExtensions.GetMimeType("xxx").ShouldBeNull();
    }
  }
}