using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryPackagingAssignment;

//TODO initial implementation to use static class for POC
internal static class TaxService {

    private static List<TaxBracket> _taxBrackets = new List<TaxBracket>() {
        new TaxBracket(0, 18200, 0),
        new TaxBracket(18200, 37000, 0.19),
        new TaxBracket(37000, 87000, 0.325),
        new TaxBracket(87000, 180000, 0.37),
        new TaxBracket(180000, int.MaxValue, 0.47)
    };

    public static int CalculateTaxableIncome(double grossIncome) {
        return (int)Math.Floor(grossIncome - SuperannuationService.CalculateSuperannuationContribution(grossIncome, true));
    }

    public static int CalculateIncomeTax(double income, bool InclusiveOfSuperannuation = false) {
        double taxableIncome = InclusiveOfSuperannuation ? CalculateUnroundedTaxableIncome(income) : income;

        var incomeTax = 0d;
        foreach (var taxBracket in _taxBrackets) {
            var taxBracketRange = taxBracket.EndRange - taxBracket.StartRange;
            if (taxableIncome <= taxBracketRange) {
                incomeTax += taxableIncome * taxBracket.TaxPercentage;
                break;
            }

            incomeTax += taxBracketRange * taxBracket.TaxPercentage;
            taxableIncome -= taxBracketRange;
        }

        return (int)Math.Ceiling(incomeTax);
    }

    public static double CalculatePayPacket(double taxableIncome, char payFrequency) {
        if (payFrequency != 'M' && payFrequency != 'F' && payFrequency != 'W') {
            throw new ArgumentException("Pay Frequency must be 'M', 'F', or 'W'");
        }

        if (payFrequency == 'M') {
            return Math.Ceiling(taxableIncome / 12 * 100) / 100;
        }

        if (payFrequency == 'F') {
            return Math.Ceiling(taxableIncome / 26 * 100) / 100;
        }

        return Math.Ceiling(taxableIncome / 52 * 100) / 100;
    }

    public static int CalculateMedicareLevy(double taxableIncome) {
        if (taxableIncome <= 21335) {
            return 0;
        }

        if (taxableIncome <= 26668) {
            return (int)Math.Ceiling((taxableIncome - 21335) * 0.1);
        }

        return (int)Math.Ceiling((taxableIncome) * 0.02);
    }

    public static int CalculateBudgetRepairLevy(double taxableIncome) {
        if (taxableIncome <= 180000) {
            return 0;
        }

        return (int)Math.Ceiling((taxableIncome - 180000) * 0.02);
    }

    internal static double CalculateUnroundedTaxableIncome(double grossIncome) {
        return grossIncome / (1 + SuperannuationService.minimumSuperPercentage);
    }
}

internal class TaxBracket {
    public int StartRange { get; set; }
    public int EndRange { get; set; }
    public double TaxPercentage { get; set; }

    internal TaxBracket(int startRange, int endRange, double taxPercentage) {
        StartRange = startRange;
        EndRange = endRange;
        TaxPercentage = taxPercentage;
    }
}