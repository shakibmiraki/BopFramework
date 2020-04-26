
using Bop.Core.Domain.Customers;

namespace Bop.Core.Configuration
{

    /// <summary>
    /// Represents single page application configuration parameters
    /// </summary>

    public partial class SpaConfig
    {

        public string AccessTokenObjectKey { get; set; }

        public string RefreshTokenObjectKey { get; set; }

        public string LoginPath { get; set; }

        public string LogoutPath { get; set; }

        public string RegisterPath { get; set; }

        public string ActivatePath { get; set; }

        public string ResendPath { get; set; }

        public string CardAuthPath { get; set; }

        public string VerifyAccessPath { get; set; }

        public string RefreshTokenPath { get; set; }

        public string EnableCardPath { get; set; }

        public string DisableCardPath { get; set; }

        public string GetCardsPath { get; set; }

        public string ExportLanguagePath { get; set; }

        public string GetAllLanguagePath { get; set; }

        public string GetReceiptsPath { get; set; }

        public string AdminRoleName { get; set; } = BopCustomerDefaults.AdministratorsRoleName;

        public string CorsPath { get; set; }
    }
}
