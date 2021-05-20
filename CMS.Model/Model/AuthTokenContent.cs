using CMS.Model.Enum;
using System.Threading;

namespace CMS.Model.Model
{
    public class AuthTokenContent
    {
        public int UserId { get; set; }

        public UserType UserType { get; set; }

        private static readonly AsyncLocal<AuthTokenContent> _current = new AsyncLocal<AuthTokenContent>();

        public static AuthTokenContent Current
        {
            get => _current.Value;
            set => _current.Value = value;
        }
    }
}
