using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SuivA.Windows
{
    public abstract class ViewModel : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName] => OnValidate(columnName);

        [Obsolete] public string Error => throw new NotSupportedException();

        protected virtual string OnValidate(string propertyName)
        {
            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var results = new Collection<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, context, results, true);

            if (isValid) return null;
            var result = results.SingleOrDefault(p => p.MemberNames.Any(
                memberName => memberName == propertyName));


            return result?.ErrorMessage;
        }
    }
}