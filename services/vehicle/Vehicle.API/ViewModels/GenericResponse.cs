namespace Vehicle.API.ViewModels
{
    public class GenericResponse
    {
        public static ResultModel GenericApplicationError(dynamic data)
        {
            return new ResultModel
            {
                Message = "Internal server error, please try again later",
                Success = false,
                MetaData = data
            };
        }

        public static ResultModel DomainError(string message, IReadOnlyCollection<string> errors)
        {
            return new ResultModel
            {
                Message = message,
                Success = false,
                MetaData = errors
            };

        }
    }
}
