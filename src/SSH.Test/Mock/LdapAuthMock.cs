using SSH.Core.DTO;
using SSH.Core.Enum;

namespace SSH.Test.Mock
{
    public class LdapAuthMock
    {

        public static AuthDTO LdapAuthDTO()
        {
            var authdto = new AuthDTO()
            {
                EmailId = "fh_saifullah.iqbal",
                Password = "admin"
            };

            return authdto;
        }
    }
}
