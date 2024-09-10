﻿using System.Linq;
using DTKH2024.SbinSolution.Editions;
using DTKH2024.SbinSolution.EntityFrameworkCore;
using DTKH2024.SbinSolution.ExtraProperties;
using DTKH2024.SbinSolution.MultiTenancy.Payments;

namespace DTKH2024.SbinSolution.Test.Base.TestData
{
    public class TestSubscriptionPaymentBuilder
    {
        private readonly SbinSolutionDbContext _context;
        private readonly int _tenantId;

        public TestSubscriptionPaymentBuilder(SbinSolutionDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreatePayments();
        }

        private void CreatePayments()
        {
            var defaultEdition = _context.Editions.First(e => e.Name == EditionManager.DefaultEditionName);

            CreatePayment(1, defaultEdition.Id, _tenantId, 1, "147741");
            CreatePayment(19, defaultEdition.Id, _tenantId, 30, "1477419");
        }

        private void CreatePayment(decimal amount, int editionId, int tenantId, int dayCount, string paymentId)
        {
            var payment = new SubscriptionPayment
            {
                TenantId = tenantId,
                DayCount = dayCount,
                ExternalPaymentId = paymentId
            };

            _context.SubscriptionPayments.Add(payment);
            _context.SaveChanges();

            var product = new SubscriptionPaymentProduct(
                payment.Id,
                "Test product",
                amount,
                1,
                amount,
                new ExtraPropertyDictionary
                {
                    ["EditionId"] = editionId
                });

            _context.SubscriptionPaymentProducts.Add(product);
            _context.SaveChanges();
        }
    }
}