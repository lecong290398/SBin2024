﻿using System;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;
using DTKH2024.SbinSolution.Authorization.Accounts.Dto;

namespace DTKH2024.SbinSolution.Web.Models.Account
{
    public class EmailChangeRequestViewModel : ChangeEmailInput
    {
        /// <summary>
        /// Tenant id.
        /// </summary>
        public int? TenantId { get; set; }

        protected override void ResolveParameters()
        {
            base.ResolveParameters();

            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["tenantId"] != null)
                {
                    TenantId = Convert.ToInt32(query["tenantId"]);
                }
            }
        }
    }
}