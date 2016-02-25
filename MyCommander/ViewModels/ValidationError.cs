namespace MyCommander
{
    public class ValidationError
    {
        public ValidationError(string propertyName, string message)
        {
            this.PropertyName = propertyName;
            this.Message = message;
        }

        public string PropertyName { get; set; }

        public string Message { get; set; }
    }
}
