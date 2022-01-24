
namespace Store.Service.Common;

public class ErrorCodes
{
    public const int CommonError = 81000;
    public const int RequestArgumentInvalid = 81001;
    public const int JwtSettingsIsNull = 81002;
    public const int InvalidParameters = 81003;

    public sealed class AuthService
    {
        public const int EmptyLoginOrPass = 82001;
        public const int WrongLoginOrPass = 82002;
        public const int UserNotFound = 82003;
        public const int EmptyPassword = 82004;

        public const int AlreadyExist = 82005;
        public const int PasswordsNotEqual = 82006;
        public const int UserRoleNotFound = 82007;
        public const int CreatedUserIsNull = 82008;
    }

    public sealed class UserService
    {
        public const int EmptyLoginOrPass = 83001;
        public const int UserNotFound = 83003;
        public const int EmptyPassword = 83004;

        public const int AlreadyExist = 83005;
        public const int PasswordsNotEqual = 83006;
        public const int UserRoleNotFound = 82007;
        public const int CreatedUserIsNull = 83008;
    }
    
    public sealed class UserRepository
    {
        public const int MoreThanOneUserByPhone = 82201;
        public const int MoreThanOneUserByEmail = 82202;
        public const int UserNotFound = 82203;
        public const int UserAlreadyHaveRole = 82204;
    }

    public sealed class StoreRepository
    {
        public const int MemberNotFound = 82300;
        public const int CreatedTeamIsNull = 82301;
    }

    public sealed class StoreService
    {
        public const int EmptyUserId = 82310;
        public const int UserNotFound = 82311;
        public const int StoreNotFound = 82312;
        public const int EmptyTitle = 82313;
    }

    public sealed class CategoryService
    {
        public const int EmptyUserId = 82320;
        public const int UserNotFound = 82321;
        public const int CategoryTreeNotFound = 82322;
        public const int CategoryTreeIdIsEmpty = 82323;
        public const int CategoryTreeIdIsNotEmpty = 82324;
    }
}
