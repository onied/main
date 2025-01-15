namespace Storage.Abstractions;

public interface ITemporaryStorageCleanerService
{
    public Task CleanTemporaryStorage();
}
