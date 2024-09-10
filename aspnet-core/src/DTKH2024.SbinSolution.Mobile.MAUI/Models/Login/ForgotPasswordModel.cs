﻿using DTKH2024.SbinSolution.Validation;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Models.Login
{
    public class ForgotPasswordModel
    {
        private string _emailAddress;

        public bool IsForgotPasswordDisabled { get; set; }

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                SetForgotPasswordStatus();
            }
        }

        private void SetForgotPasswordStatus()
        {
            IsForgotPasswordDisabled = !ValidationHelper.IsEmail(EmailAddress);
        }
    }
}
