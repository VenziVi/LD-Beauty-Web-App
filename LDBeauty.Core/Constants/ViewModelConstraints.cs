namespace LDBeauty.Core.Constants
{
    public static class ViewModelConstraints
    {
        public const string MinMaxLengthError = "The {0} must be at least {2} and max {1} characters long.";
        
        public const string PhoneError = "Phone number must be 10 digits long.";

        public const string PriceError = "Psice must be with min value 1,00 and max value 999,99.";

        public const string QuantityError = "Quantity must be between 0 and 100.";
    }
}
