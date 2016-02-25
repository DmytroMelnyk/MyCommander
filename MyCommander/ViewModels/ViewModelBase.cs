using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MyCommander
{
    [Serializable]
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Dictionary<string, ValidationContext> validationDictionary;

        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasErrors
        {
            get { return this.Errors.Any(); }
        }

        protected LinkedList<ValidationError> Errors { get; private set; } = new LinkedList<ValidationError>();

        private Dictionary<string, ValidationContext> ValidationDictionary
        {
            get
            {
                return this.validationDictionary ?? (this.validationDictionary = this.GetType().
                    GetProperties().
                    Where(p => p.CanWrite && p.IsDefined(typeof(ValidationAttribute), true)).
                    ToDictionary(p => p.Name, p => new ValidationContext(this)
                    {
                        MemberName = p.Name
                    }));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return this.Errors.Where(e => e.PropertyName == propertyName).Select(e => e.Message);
        }

        protected virtual IEnumerable<string> GetErrorMessages<T>(string propertyName, T propertyValue)
        {
            ValidationContext context;
            if (!this.ValidationDictionary.TryGetValue(propertyName, out context))
            {
                return null;
            }

            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(propertyValue, context, results);
            return results.Select(item => item.ErrorMessage);
        }

        protected bool ValidateProperty<T>(T propertyValue, bool notifyErrorChanged = true, [CallerMemberName]string propertyName = null)
        {
            var messages = this.GetErrorMessages(propertyName, propertyValue);
            if (messages == null)
            {
                return true;
            }

            if (notifyErrorChanged)
            {
                foreach (var error in this.Errors.Where(e => e.PropertyName == propertyName).ToArray())
                {
                    this.Errors.Remove(error);
                }

                foreach (string message in messages)
                {
                    this.Errors.AddFirst(new ValidationError(propertyName, message));
                }

                this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }

            return !messages.Any();
        }

        protected bool Set<T>(ref T field, T propertyValue, bool validateProperty = true, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, propertyValue))
            {
                return false;
            }

            field = propertyValue;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (validateProperty)
            {
                this.ValidateProperty(propertyValue, true, propertyName);
            }

            return true;
        }
    }
}
