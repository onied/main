namespace Storage.Abstractions;

public interface IStorageService
{
    Task<IResult> Upload(IFormFileCollection files);
    Task<IResult> Get(string objectName);
    Task<IResult> GetUrlByFileId(string fileId);
    Task<IResult> GetMetadata(string fileId);
}
