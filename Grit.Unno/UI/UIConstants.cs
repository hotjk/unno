using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public static class UIConstants
    {
        /// <summary>
        /// CompanyInfo-0_FinancialReport-[id]_Year-0
        ///                               ^^^^
        /// </summary>
        public const string ID = "[id]";
        /// <summary>
        /// CompanyInfo-0_FinancialReport-0_Year-0
        ///              ^
        /// </summary>
        public static readonly char[] SEPRATOR_NODE = new char[] { '_' };
        /// <summary>
        /// CompanyInfo-0_FinancialReport-0_Year-0
        ///            ^
        /// </summary>
        public static readonly char[] SEPRATOR_INDEX = new char[] { '-' };
        /// <summary>
        /// CompanyInfo-0_FinancialReport-0_Year-0 // 1
        /// CompanyInfo-0_FinancialReport-0_GrossProfit-0 // 2
        /// ...
        /// CompanyInfo-0_FinancialReport-0_NetProfit-0 // n <= MaxHttpCollectionKeys
        /// </summary>
        public const int MaxHttpCollectionKeys = 1000;
        /// <summary>
        /// "CompanyInfo-0_FinancialReport-0_Year-0".Length <= MaxUnnoKeyLength
        /// </summary>
        public const int MaxUnnoKeyLength = 100;
    }
}
