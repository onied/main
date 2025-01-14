namespace Storage.Abstractions;

public interface IPermanentStorageTransferService
{
    public Task TransferAfterUpload(string fileId);
}
