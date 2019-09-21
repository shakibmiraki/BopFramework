using System;
using System.ComponentModel.DataAnnotations.Schema;
using Bop.Core.Caching;
using Bop.Core.Domain.Localization;


namespace Bop.Services.Localization
{
    /// <summary>
    /// Language (for caching)
    /// </summary>
    [Serializable]
    //Entity Framework will assume that any class that inherits from a POCO class that is mapped to a table on the database requires a Discriminator column
    //That's why we have to add [NotMapped] as an attribute of the derived class.
    [NotMapped]
    public class LanguageForCaching : Language, IEntityForCaching
    {
        public LanguageForCaching()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="l">Language to copy</param>
        public LanguageForCaching(Language l)
        {
            Id = l.Id;
            Name = l.Name;
            LanguageCulture = l.LanguageCulture;
            UniqueSeoCode = l.UniqueSeoCode;
            FlagImageFileName = l.FlagImageFileName;
            Rtl = l.Rtl;
            DefaultCurrencyId = l.DefaultCurrencyId;
            Published = l.Published;
            DisplayOrder = l.DisplayOrder;
        }
    }
}
