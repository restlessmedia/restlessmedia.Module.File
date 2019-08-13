using restlessmedia.Test;
using Should;

namespace restlessmedia.Module.File.UnitTest
{
  public class MimeTests
  {
    [Fact]
    public void TestJpgExtensionWithoutPeriodReturnsJpgMimeType()
    {
      MimeExtensions.GetMimeType("jpg").MustBe("image/jpeg");
    }

    [Fact]
    public void TestJpgExtensionWithPeriodReturnsJpgMimeType()
    {
      MimeExtensions.GetMimeType(".jpg").MustBe("image/jpeg");
    }

    [Fact]
    public void TestNonExistentExtensionReturnsNull()
    {
      MimeExtensions.GetMimeType("xxx").ShouldBeNull();
    }
  }
}