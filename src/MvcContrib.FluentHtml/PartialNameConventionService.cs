using System;

namespace MvcContrib.FluentHtml
{
    /// <summary>
    /// provides the naming service and ability to change naming convention for typed partials
    /// </summary>
    public static class PartialNameConventionService
    {
        private static string _partialNameConvention = "{0}TypePartial";

        /// <summary>
        /// Partial name convention for strongly typed views.
        /// </summary>
        public static string PartialNameConvention
        {
            get { return _partialNameConvention; }
            set { _partialNameConvention = value; }
        }

        /// <summary>
        /// Creates a partial name based on the name convention
        /// </summary>
        /// <param name="targetType">the targeted type to be rendered</param>
        /// <returns>name according to convention</returns>
        public static string GeneratePartialName(Type targetType)
        {
            return String.Format(_partialNameConvention, targetType.Name);
        }
    }
}
