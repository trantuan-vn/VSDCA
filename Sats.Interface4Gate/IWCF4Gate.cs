using System.ServiceModel;
using Sats.Utils;

namespace Sats.Interface4Gate
{
    [ServiceContract]
    public interface IWCF4Gate
    {
        [OperationContract]
        long SendMessage(ref string message);
        [OperationContract]
        DataContainer GetData(string sql);
        [OperationContract]
        DataContainer GetSearch(string message);
        [OperationContract]
        DataContainer CreateReport(string message);
    }
}
