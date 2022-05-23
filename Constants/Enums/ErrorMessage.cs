using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants.Enums;

public enum ErrorMessage
{
    [LocalizedDescription("EmailInvalid", typeof(ErrorMessage))]
    EmailInvalid,
    [LocalizedDescription("PhoneNumberInvalid", typeof(ErrorMessage))]
    PhoneNumberInvalid,
    [LocalizedDescription("UserNameInvalid", typeof(ErrorMessage))]
    UserNameInvalid,
    [LocalizedDescription("PasswordInvalid", typeof(ErrorMessage))]
    PasswordInvalid,
}