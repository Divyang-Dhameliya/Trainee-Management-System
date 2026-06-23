public class FileStorageOptions
{
    public string RootPath { get; set;} = null!;
    public int MaxFileSizeMb { get; set; }
    public List<string> AllowedExtensions { get; set; } = [];
}