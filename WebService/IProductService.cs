using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProductService" in both code and config file together.
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        Product GetProduct(string name);
        [OperationContract]
        bool RegisterUser(string username, string password);
        [OperationContract]
        Customer GetUser(string username, string password);
        [OperationContract]
        List<Product> GetProducts();
        [OperationContract]
        bool BuyProduct(string product, string username, string password);
        [OperationContract]
        List<CustomerProduct> GetOrders(string username, string password);
    }
}
