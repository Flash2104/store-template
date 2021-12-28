using Store.Service.Contracts.Store.Get;
using Store.Service.Contracts.Store.Update;

namespace Store.Service.Contracts.Store;

public interface IStoreService
{
    Task<GetStoreResponse> Get();

    Task<UpdateStoreResponse> Update(UpdateStoreRequest request);
}