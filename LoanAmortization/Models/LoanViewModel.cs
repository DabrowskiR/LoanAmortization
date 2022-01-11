using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoanAmortization.Models
{
    public class LoanViewModel
    {
        [Display(Name = "Initial principal balance")]
        public decimal InitialPrincipalBalance { get; set; }
        
        [Range(0, 100)]
        [Display(Name = "Annual interest rate (%)")]
        public int AnnualInterestRate { get; set; }

        [Display(Name = "Loan length (in months)")]
        public int LoanLength { get; set; }

        [Display(Name = "Inception date")]
        [DataType(DataType.Date)]
        public DateTime InceptionDate { get; set; }
    }
}
