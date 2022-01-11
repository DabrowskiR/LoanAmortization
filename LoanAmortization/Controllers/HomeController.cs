using LoanAmortization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoanAmortization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAmortizationTable(LoanViewModel lvm)
        {
            decimal balance = lvm.InitialPrincipalBalance;
            List<InstallmentViewModel> installments = new List<InstallmentViewModel>();
            decimal avgWeekLength = 365m / 52m;
            decimal interestRate = (lvm.AnnualInterestRate / 100.00m) / 365m * avgWeekLength;
            decimal weekPayment = GetWeklyPayment(lvm, interestRate, avgWeekLength);

            int fullPayments = (int)(((lvm.LoanLength / 12m) * 52m) - ((lvm.LoanLength / 12m) * 52m % 1));
            for (int i = 0; i < fullPayments; i++)
            {
                decimal interestPortion = interestRate * balance;
                decimal principalPortion = weekPayment - interestPortion;

                installments.Add(new InstallmentViewModel() 
                { 
                    PaymentNumber = i + 1, 
                    PrincipalPortion = Math.Round(principalPortion, 2), 
                    InterestPortion = Math.Round(interestPortion, 2), 
                    PaymentDate = lvm.InceptionDate.AddDays((i+1) * 7), 
                    PrincipalBalanceLeft = Math.Round(balance - principalPortion, 2) 
                });
                balance -= principalPortion;
            }
            if((lvm.LoanLength / 12m * 52m) % 1 != 0)
            {
                decimal interestPortion = interestRate * balance;

                installments.Add(new InstallmentViewModel()
                {
                    PaymentNumber = fullPayments+1,
                    PrincipalPortion = Math.Round(balance, 2),
                    InterestPortion = Math.Round(interestPortion, 2),
                    PaymentDate = lvm.InceptionDate.AddDays((fullPayments + 1) * 7),
                    PrincipalBalanceLeft = Math.Round(balance - balance, 2)
                });
                balance -= balance;
            }

            return PartialView("InstallmentList", installments);
        }

        private decimal GetWeklyPayment(LoanViewModel lvm, decimal i, decimal avgWeekLenght)
        {
            decimal p = lvm.InitialPrincipalBalance;
            decimal n = ((lvm.LoanLength / 12m) * 52m);

            return p * (i / (1 - (decimal)Math.Pow((1 + (double)i), (double)-n)));
        }

        public IActionResult OnGetPartial()
        {
            return PartialView("InstallmentList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
