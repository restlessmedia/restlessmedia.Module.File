namespace restlessmedia.Module.File.Configuration
{
  public interface IFileSize
  {
    string Name { get; }

    int? Width { get; }

    int? Height { get; }

    int Quality { get; }

    string Mode { get; }

    string Scale { get; }

    string BgColor { get; }

    string Anchor { get; }

    bool IsPreset { get; }
  }
}