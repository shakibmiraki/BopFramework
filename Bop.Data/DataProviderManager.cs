using Bop.Core;
using Bop.Core.Infrastructure;
using Bop.Data;

namespace Bop.Data
{
    /// <summary>
    /// Represents the data provider manager
    /// </summary>
    public partial class DataProviderManager : IDataProviderManager
    {
        #region Methods

        /// <summary>
        /// Gets data provider by specific type
        /// </summary>
        /// <param name="dataProviderType">Data provider type</param>
        /// <returns></returns>
        public static IBopDataProvider GetDataProvider(DataProviderType dataProviderType)
        {
            switch (dataProviderType)
            {
                case DataProviderType.SqlServer:
                    return new MsSqlNopDataProvider();
                case DataProviderType.MySql:
                    return new MySqlNopDataProvider();
                default:
                    throw new BopException($"Not supported data provider name: '{dataProviderType}'");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        public IBopDataProvider DataProvider
        {
            get
            {
                var dataProviderType = Singleton<DataSettings>.Instance.DataProvider;

                return GetDataProvider(dataProviderType);
            }
        }

        #endregion
    }
}
