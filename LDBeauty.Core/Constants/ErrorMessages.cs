namespace LDBeauty.Core.Constants
{
    public static class ErrorMessages
    {
        public const string DatabaseConnectionError = "There is a problem with database connection, please try again later!";

        public const string OutOfStockError = "You can't make an order becouse one or more products are out of stock!";

        public const string ImageNotFound = "Image not found, please try again later.";

        public const string SomethingWrong = "Something went wrong, please try again later.";

        public const string ProductNotFound = "Product not found, please try again later.";

        public const string ImageExists = "Image already exists in favourites!";

        public const string ProductEists = "Product already exists in favourites!";

        public static string ProductAddedToFavoutites = "Product was added successfuly";

        public static string MinCharacters = "You must add min 4 character.";

        public const string MissingProduct = "There are no products with that name.";

        public const string RemovedProduct = "Product is already removed from favourites.";

        public const string RemovedImage = "Image is already removed from favourites.";
    }
}
