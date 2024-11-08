namespace SalaryPackagingAssignment;

//TODO initial implementation to use static class for POC
internal static class SuperannuationService {
    internal static readonly double minimumSuperPercentage = 0.095;

    public static double CalculateSuperannuationContribution(double salary, bool inclusiveOfSuper = true) {
        if (inclusiveOfSuper) {
            var taxableIncome = TaxService.CalculateUnroundedTaxableIncome(salary);
            return Math.Ceiling((salary - taxableIncome) * 100) / 100;
        }

        return (int)Math.Ceiling((salary * minimumSuperPercentage) * 100) / 100;
    }
}