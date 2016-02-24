using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCommander
{
    [Serializable]
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        Dictionary<string, ValidationContext> _validationDictionary;
        Dictionary<string, ValidationContext> ValidationDictionary
        {
            get
            {
                return _validationDictionary ?? (_validationDictionary = GetValidationDictionary());
            }
        }

        Dictionary<string, ValidationContext> GetValidationDictionary()
        {
            var propertiesWithValidationAttribute = GetType().GetProperties().
                Where(property => property.CanWrite && property.IsDefined(typeof(ValidationAttribute), true));

            var _validationDictionary = new Dictionary<string, ValidationContext>();
            foreach (var property in propertiesWithValidationAttribute)
                _validationDictionary.Add(property.Name, new ValidationContext(this) { MemberName = property.Name });

            return _validationDictionary;
        }

        protected LinkedList<ValidationError> Errors { get; private set; } = new LinkedList<ValidationError>();

        public IEnumerable GetErrors(string propertyName)
        {
            return Errors.Where(e => e.PropertyName == propertyName).Select(e => e.Message);
        }

        public bool HasErrors
        {
            get { return Errors.Any(); }
        }

        protected virtual IEnumerable<string> GetErrorMessages<T>(string propertyName, T propertyValue)
        {
            ValidationContext context;
            if (!ValidationDictionary.TryGetValue(propertyName, out context))
                return null;

            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(propertyValue, context, results);
            return results.Select(item => item.ErrorMessage);
        }

        void ValidateProperty<T>(string propertyName, T propertyValue)
        {
            var messages = GetErrorMessages(propertyName, propertyValue);
            if (messages == null)
                return;

            foreach (var error in Errors.Where(e => e.PropertyName == propertyName).ToArray())
                Errors.Remove(error);

            foreach (string message in messages)
                Errors.AddFirst(new ValidationError(propertyName, message));

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void Set<T>(ref T field, T propertyValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, propertyValue))
            {
                field = propertyValue;
                ValidateProperty(propertyName, propertyValue);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
