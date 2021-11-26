using System.Threading.Tasks;

namespace Stencil.Native.Commanding
{
    public interface ICommandField
    {
        string GroupName { get; }
        string FieldName { get; }
        bool IsRequired { get; }

        string GetFieldValue();
        void SetFieldValue(string value);

        /// <summary>
        /// Validates input for the field itself, not related to business logic. 
        /// :> Validated here: Email was provided and properly formatted
        /// :> Not Validated here:  Email already exists in system
        /// </summary>
        Task<string> ValidateUserInputAsync();

    }
}
