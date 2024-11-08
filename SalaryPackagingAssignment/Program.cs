using SalaryPackagingAssignment;

//TODO DI should be used for services not static classes

Console.WriteLine("Enter your salary package amount: ");
var salaryPackageInput = Console.ReadLine();
if (!double.TryParse(salaryPackageInput, out double salaryPackage)) {
    //TODO this could be handled better by asking for re-input and allowing user an option to close application
    Console.Error.WriteLine("Salary package provided must be a number");
    return;
}

Console.WriteLine("Enter your pay frequency (W for weekly, F for fortnightly, M for monthly): ");
var payFrequencyInput = Console.ReadLine();
//TODO this should also handle lowercase inputs left as uppercase for POC
if (payFrequencyInput != "W" && payFrequencyInput != "F" && payFrequencyInput != "M") {
    //TODO this could be handled better by asking for re-input and allowing user an option to close application
    Console.Error.WriteLine("Pay frequency must be 'W' for weekly, 'F' for fornightly or 'M' for monthly");
    return;
}

char payFrequency = payFrequencyInput[0];

var taxableIncome = TaxService.CalculateTaxableIncome(salaryPackage);
var superannuation = SuperannuationService.CalculateSuperannuationContribution(salaryPackage);
var incomeTax = TaxService.CalculateIncomeTax(salaryPackage);
var payPacket = TaxService.CalculatePayPacket(taxableIncome, payFrequency);

Console.WriteLine("Calculating salay details...");

Console.WriteLine($"Gross package: {salaryPackage}");
Console.WriteLine($"Superannuation: {superannuation}");
Console.WriteLine();

var medicareLevy = TaxService.CalculateMedicareLevy(taxableIncome);
var budgetRepairLevy = TaxService.CalculateBudgetRepairLevy(taxableIncome);

Console.WriteLine("Deductions:");
Console.WriteLine($"Medicare Levy: {medicareLevy}");
Console.WriteLine($"Budget Repair Levy: {budgetRepairLevy}");
Console.WriteLine($"Income Tax: {incomeTax}");
Console.WriteLine();

Console.WriteLine($"Net Income: {taxableIncome - incomeTax}");
Console.WriteLine($"Pay packet: {payPacket}");
Console.WriteLine("Press any key to end...");