using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace DTKH2024.SbinSolution.Net.Emailing
{
    public class SbinSolutionSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public SbinSolutionSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}