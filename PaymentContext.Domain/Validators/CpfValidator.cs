namespace PaymentContext.Domain.Validators
{
    public static class CpfValidator
    {
        public static bool IsValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            var numbers = cpf.Select(c => c - '0').ToArray();

            int d1 = CalcDigit(numbers, 9);
            int d2 = CalcDigit(numbers, 10);

            return d1 == numbers[9] && d2 == numbers[10];
        }

        private static int CalcDigit(int[] numbers, int length)
        {
            int sum = 0, mult = length + 1;
            for (int i = 0; i < length; i++)
                sum += numbers[i] * mult--;

            int rest = sum % 11;
            return rest < 2 ? 0 : 11 - rest;
        }
    }
}