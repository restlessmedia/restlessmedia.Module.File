using System.Configuration;

namespace restlessmedia.Module.File.Configuration
{
  public class FileSize : ConfigurationElement, IFileSize
  {
    [ConfigurationProperty(nameProperty, IsRequired = true)]
    public string Name
    {
      get
      {
        return (string)this[nameProperty];
      }
    }

    /// <summary>
    /// The file width - this isn't required as either width or height could be omitted to enable scaling
    /// </summary>
    [ConfigurationProperty(widthProperty, IsRequired = false)]
    public int? Width
    {
      get
      {
        return (int?)this[widthProperty];
      }
    }

    /// <summary>
    /// The file height - this isn't required as either width or height could be omitted to enable scaling
    /// </summary>
    [ConfigurationProperty(heightProperty, IsRequired = false)]
    public int? Height
    {
      get
      {
        return (int?)this[heightProperty];
      }
    }

    [ConfigurationProperty(qualityProperty, IsRequired = false, DefaultValue = 100)]
    public int Quality
    {
      get
      {
        return (int)this[qualityProperty];
      }
    }

    /// <summary>
    /// crop|stretch|carve|pad
    /// </summary>
    [ConfigurationProperty(modeProperty, IsRequired = false, DefaultValue = null)]
    public string Mode
    {
      get
      {
        return (string)this[modeProperty];
      }
    }

    /// <summary>
    /// down|both|canvas
    /// </summary>
    [ConfigurationProperty(scaleProperty, IsRequired = false, DefaultValue = null)]
    public string Scale
    {
      get
      {
        return (string)this[scaleProperty];
      }
    }

    /// <summary>
    /// Background color
    /// </summary>
    [ConfigurationProperty(bgColorProperty, IsRequired = false, DefaultValue = null)]
    public string BgColor
    {
      get
      {
        return (string)this[bgColorProperty];
      }
    }

    /// <summary>
    /// anchor=topleft|topcenter|topright|middleleft|middlecenter|middleright|bottomleft|bottomcenter|bottomright How to anchor the image when padding or cropping. (new in V3.1)
    /// </summary>
    [ConfigurationProperty(anchorProperty, IsRequired = false, DefaultValue = null)]
    public string Anchor
    {
      get
      {
        return (string)this[anchorProperty];
      }
    }

    [ConfigurationProperty(isPresetProperty, IsRequired = false, DefaultValue = false)]
    public bool IsPreset
    {
      get
      {
        return (bool)this[isPresetProperty];
      }
    }

    private const string nameProperty = "name";

    private const string widthProperty = "width";

    private const string heightProperty = "height";

    private const string qualityProperty = "quality";

    private const string modeProperty = "mode";

    private const string scaleProperty = "scale";

    private const string bgColorProperty = "bgcolor";

    private const string anchorProperty = "anchor";

    private const string isPresetProperty = "isPreset";
  }
}