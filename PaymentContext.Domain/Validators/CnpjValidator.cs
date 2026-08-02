namespace PaymentContext.Domain.Validators
{
    public static class CnpjValidator
    {
        public static bool IsValid(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                return false;

            // Rejeita sequências repetidas (00000000000000, 11111111111111, etc.)
            if (cnpj.Distinct().Count() == 1)
                return false;

            var numbers = cnpj.Select(c => c - '0').ToArray();

            int d1 = CalculateCheckDigit(numbers, 12);
            if (d1 != numbers[12])
                return false;

            int d2 = CalculateCheckDigit(numbers, 13);
            if (d2 != numbers[13])
                return false;

            return true;
        }

        private static int CalculateCheckDigit(int[] numbers, int length)
        {
            int sum = 0;
            int[] multipliers = length == 12
                ? [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]
                : [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            for (int i = 0; i < length; i++)
                sum += numbers[i] * multipliers[i];

            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }
    }
}