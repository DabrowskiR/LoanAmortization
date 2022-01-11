using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanAmortization.Models
{
    public class InstallmentViewModel
    {
        [Display(Name = "Payment number")]
        public int PaymentNumber { get; set; }

        [Display(Name = "Payment date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Interest portion")]
        public decimal InterestPortion { get; set; }

        [Display(Name = "Principal portion")]
        public decimal PrincipalPortion { get; set; }

        [Display(Name = "Principal balance left")]
        public decimal PrincipalBalanceLeft { get; set; }
    }
}
