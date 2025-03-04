using System;
using System.Linq;

namespace ABS_MathGenerator
{
    public class Question
    {
        public string QuestionText { get; set; } // The text of the question
        public string Answer { get; set; }       // The answer to the question

        // Constructor to initialize a question with text and answer
        public Question(string questionText, string answer)
        {
            QuestionText = questionText;
            Answer = answer;
        }

        // Optionally, you can add other properties or methods here (e.g., feedback, hints, difficulty)
    }

    public class Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public double Value { get; set; }

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            Value = (double)numerator / denominator;
        }

        // ToString Method: Check if it's a mixed fraction or a simple fraction
        public override string ToString()
        {
            int wholeNumber = Numerator / Denominator;  // Whole number part
            int remainder = Numerator % Denominator;   // Remainder part of the fraction

            if(Denominator == 1)
            {
                return wholeNumber.ToString();
            }
            if (wholeNumber > 0 && remainder != 0)
            {
                return $"{wholeNumber} {remainder}/{Denominator}"; // Mixed fraction
            }
            else if (wholeNumber > 0)
            {
                return $"{wholeNumber}"; // Only whole number
            }
            else
            {
                return $"{Numerator}/{Denominator}"; // Simple fraction
            }
        }


    }

    public abstract class QuestionGenerator
    {
        protected static readonly Random _random = new Random();

        // Generate a random decimal (1 to 2 decimal places)
        protected decimal GenerateDecimal(int min = -10, int max = 10)
        {
            decimal value = (decimal)_random.NextDouble() * _random.Next(min, max + 1);
            return value;
        }

        // Generate a random fraction with an optional flag for same denominator
        protected Fraction GenerateFraction(int minNumerator = -10,int maxNumerator = 10, int maxDenominator = 10, bool sameDenominator = false, int requiredDenominator = 1)
        {
            int denominator = _random.Next(2, maxDenominator + 1);  // Random denominator
            int numerator = _random.Next(minNumerator, Math.Min(maxNumerator + 1, denominator));


            // If sameDenominator is true, use a common denominator for all fractions
            if (sameDenominator)
            {
                denominator = requiredDenominator;  // Use the required common denominator
                numerator = _random.Next(1, Math.Min(maxNumerator + 1, denominator));  // Ensure numerator is <= denominator
            }

            // Ensure numerator is always <= denominator (when not using sameDenominator)
            else
            {
                if (Math.Abs(numerator) > denominator)
                {
                    numerator = denominator;  // Adjust the numerator to be <= denominator
                }
            }

            return new Fraction(numerator, denominator);
        }


        // Generate a mixed number with an optional flag for same denominator
        protected Fraction GenerateMixedNumber(int minWhole = -5, int maxWhole = 5, int maxNumerator = 5, int maxDenominator = 10, bool sameDenominator = false, int requiredDenominator = 1)
        {
            // Generate the whole number part (can be negative or positive)
            int whole = _random.Next(minWhole, maxWhole + 1);  // Whole number part can be negative or positive

            // Generate the numerator and denominator for the fractional part
            int numerator = _random.Next(1, maxNumerator + 1);  // Fraction numerator
            int denominator = _random.Next(2, maxDenominator + 1);  // Fraction denominator

            // If the sameDenominator flag is true, we will use the provided denominator
            if (sameDenominator)
            {
                denominator = requiredDenominator;  // Use the required common denominator
                numerator = _random.Next(1, Math.Min(maxNumerator + 1, denominator));  // Ensure numerator is <= denominator
            }

            // Ensure numerator is always less than denominator for valid fraction
            if (numerator >= denominator)
            {
                numerator = denominator - 1;  // Adjust numerator to be less than denominator
            }

            // If the whole number part is negative, adjust the numerator accordingly
            if (whole < 0)
            {
                numerator = Math.Abs(numerator);  // Keep the numerator positive (fraction part)
            }

            // Combine the whole number and fraction to form the mixed number as a single fraction
            int mixedNumerator = whole * denominator + numerator;
            return new Fraction(mixedNumerator, denominator);
        }


        // Generate a random integer (positive and negative)
        protected int GenerateInteger(int min = -10, int max = 10)
        {
            return _random.Next(min, max + 1);  // Random integer between min and max
        }

        // Generate a random absolute value problem (positive integers only)
        protected int GenerateAbsoluteValue(int min = 1, int max = 10)
        {
            return _random.Next(min, max + 1);  // Random integer, positive only
        }
        // Helper method to generate a fraction from an integer (i.e., an integer as a fraction with denominator 1)
        protected Fraction GenerateIntegerAsFraction(int minValue = -10, int maxValue = 10)
        {
            int value = _random.Next(minValue, maxValue + 1);
            return new Fraction(value, 1); // Return as a fraction with denominator 1
        }

    }

    namespace Grade6
    {
        public enum MasteryLevel
        {
            Level1, // Whole numbers
            Level2, // Integers
            Level3, // Positive & negative fractions
            Level4, // Mixed rational numbers
            Level5  // Absolute values & different number line scales
        }

        public class NumberSenseAndOperations : QuestionGenerator
        {


            public Question NSO_1_1_OrderingRationalNumbers(MasteryLevel level, bool sameDenominator)
            {
                List<object> numbers = new List<object>();  // A common list to hold numbers of any type (int, Fraction, etc.)
                List<object> sortedNumbers = new List<object>(); // Define sortedNumbers earlier for reuse
                string questionText = "";
                string answer = "";

                switch (level)
                {
                    case MasteryLevel.Level1:
                        // Generate 5 random whole numbers
                        numbers = Enumerable.Range(1, 10).OrderBy(x => _random.Next()).Take(5).Cast<object>().ToList();
                        sortedNumbers = numbers.OrderBy(n => Convert.ToDecimal(n)).ToList();  // Sorting by numeric value
                        questionText = $"Order the following whole numbers from least to greatest: {string.Join(", ", numbers)}";
                        answer = string.Join(", ", sortedNumbers);
                        break;

                    case MasteryLevel.Level2:
                        // Generate 5 random integers (both negative and positive)
                        numbers = new[] { GenerateInteger(-10, 10), GenerateInteger(-10, 10), GenerateInteger(-10, 10), GenerateInteger(-10, 10), GenerateInteger(-10, 10) }
                            .Cast<object>().ToList();
                        sortedNumbers = numbers.OrderBy(n => Convert.ToDecimal(n)).ToList();  // Sorting by numeric value
                        questionText = $"Order the following integers from least to greatest: {string.Join(", ", numbers)}";
                        answer = string.Join(", ", sortedNumbers);
                        break;

                    case MasteryLevel.Level3:
                        var fractions = new List<Fraction>();
                        for (int i = 0; i < 5; i++)
                        {
                            int randChoice = _random.Next(0, 3); // 0 = mixed number, 1 = fraction, 2 = integer
                            switch (randChoice)
                            {
                                case 0:
                                    if (sameDenominator)
                                    {
                                        fractions.Add(GenerateFraction(sameDenominator: true, requiredDenominator: requiredDenominator));
                                    }
                                    else fractions.Add(GenerateFraction());
                                    break;
                                case 1:
                                    fractions.Add(GenerateIntegerAsFraction());
                                    break;
                                default:
                                    break;
                            }
                        }
                        numbers = fractions.Cast<object>().ToList();
                        sortedNumbers = fractions.OrderBy(f => f.Value).Cast<object>().ToList();  // Sorting by value for fractions
                        questionText = $"Order the following fractions from least to greatest: {string.Join(", ", fractions.Select(f => f.ToString()))}";
                        answer = string.Join(", ", sortedNumbers.Select(f => f.ToString()));
                        break;

                    case MasteryLevel.Level4:
                        var randomNumbersLevel4 = new List<Fraction>();
                        for (int i = 0; i < 5; i++)
                        {
                            int randChoice = _random.Next(0, 3); // 0 = mixed number, 1 = fraction, 2 = integer
                            switch (randChoice)
                            {
                                case 0:
                                    randomNumbersLevel4.Add(GenerateMixedNumber(sameDenominator: sameDenominator, requiredDenominator: requiredDenominator));
                                    break;
                                case 1:
                                    randomNumbersLevel4.Add(GenerateFraction(sameDenominator: sameDenominator, requiredDenominator: requiredDenominator));
                                    break;
                                case 2:
                                    randomNumbersLevel4.Add(GenerateIntegerAsFraction());
                                    break;
                                default:
                                    break;
                            }
                        }
                        numbers = randomNumbersLevel4.Cast<object>().ToList();
                        sortedNumbers = randomNumbersLevel4.OrderBy(f => f.Value).Cast<object>().ToList();  // Sorting by value for fractions
                        questionText = $"Order the following numbers from least to greatest: {string.Join(", ", randomNumbersLevel4.Select(f => f.ToString()))}";
                        answer = string.Join(", ", sortedNumbers.Select(f => f.ToString()));
                        break;

                    case MasteryLevel.Level5:
                        var randomNumbersLevel5 = new List<object>();
                        for (int i = 0; i < 5; i++)
                        {
                            int randChoice = _random.Next(0, 4); // 0 = mixed number, 1 = fraction, 2 = integer, 3 = absolute value
                            switch (randChoice)
                            {
                                case 0:
                                    randomNumbersLevel5.Add(GenerateMixedNumber(sameDenominator: sameDenominator, requiredDenominator: requiredDenominator));
                                    break;
                                case 1:
                                    randomNumbersLevel5.Add(GenerateFraction(sameDenominator: sameDenominator, requiredDenominator: requiredDenominator));
                                    break;
                                case 2:
                                    randomNumbersLevel5.Add(GenerateIntegerAsFraction());
                                    break;
                                case 3:
                                    randomNumbersLevel5.Add(GenerateAbsoluteValue());
                                    break;
                            }
                        }
                        numbers = randomNumbersLevel5;
                        sortedNumbers = randomNumbersLevel5
                            .OrderBy(item => Math.Abs(GetDecimalValue(item)))  // Sort by absolute value of decimal representation
                            .Cast<object>().ToList();
                        questionText = $"Order the following absolute values from least to greatest: {string.Join(", ", randomNumbersLevel5.Select(item => item.ToString()))}";
                        answer = string.Join(", ", sortedNumbers.Select(item => item.ToString()));
                        break;

                    default:
                        answer = "Invalid level.";
                        questionText = "Please select a valid mastery level.";
                        break;
                }

                return new Question(questionText, answer);
            }

            public Question NSO_1_1_ComparingRationalNumbers(MasteryLevel level)
            {
                string questionText;
                string answer;

                switch (level)
                {
                    case MasteryLevel.Level1:
                        // Compare two natural numbers
                        var naturalNumber1 = GenerateInteger(1, 20); // Random natural number between 1 and 20
                        var naturalNumber2 = GenerateInteger(1, 20);
                        answer = CompareValues(naturalNumber1, naturalNumber2);
                        questionText = $"Compare the following natural numbers: {naturalNumber1} and {naturalNumber2}.";
                        break;

                    case MasteryLevel.Level2:
                        // Compare two integers, can include absolute values
                        var integer1 = GenerateInteger(-10, 10); // Random integer between -10 and 10
                        var integer2 = GenerateInteger(-10, 10);
                        answer = CompareValues(integer1, integer2);
                        questionText = $"Compare the following integers: {integer1} and {integer2}.";
                        break;

                    case MasteryLevel.Level3:
                        // Compare two decimals, can be negative, can include abs
                        var decimal1 = GenerateDecimal(-10, 10); // Random decimal between -10 and 10
                        var decimal2 = GenerateDecimal(-10, 10);
                        answer = CompareValues(decimal1, decimal2);
                        questionText = $"Compare the following decimals: {decimal1} and {decimal2}.";
                        break;

                    case MasteryLevel.Level4:
                        // Compare two fractions
                        var fraction1 = GenerateFraction();
                        var fraction2 = GenerateFraction();
                        answer = CompareValues(fraction1, fraction2);
                        questionText = $"Compare the following fractions: {fraction1} and {fraction2}.";
                        break;

                    case MasteryLevel.Level5:
                        // Compare decimal and fraction, can be negative, can be an ABS
                        var decimalForComparison = GenerateDecimal(-10, 10);
                        var fractionForComparison = GenerateFraction();
                        answer = CompareValues(decimalForComparison, fractionForComparison);
                        questionText = $"Compare the following decimal and fraction: {decimalForComparison} and {fractionForComparison}.";
                        break;

                    default:
                        answer = "Invalid level.";
                        questionText = "Please select a valid mastery level.";
                        break;
                }

                return new Question(questionText, answer);
            }

            public Question NSO_1_1_CompareRationalNumbersInRealWorldContext(MasteryLevel level,bool sameDenominator)
            {
                return new Question("","");
            }
            // Helper function to compare two values (could be integers, decimals, or fractions)
            private string CompareValues(object value1, object value2)
            {
                decimal decimalValue1 = GetDecimalValue(value1);
                decimal decimalValue2 = GetDecimalValue(value2);

                if (decimalValue1 == decimalValue2)
                {
                    return "Both values are equal.";
                }
                else if (decimalValue1 < decimalValue2)
                {
                    return $"{decimalValue1} < {decimalValue2}";
                }
                else
                {
                    return $"{decimalValue1} > {decimalValue2}";
                }
            }



           

            

        

       

            // Method to handle getting decimal value from different types
            private decimal GetDecimalValue(object item)
            {
                if (item is Fraction fraction)
                {
                    return (decimal)fraction.Value;  // Convert double to decimal for proper comparison
                }
                else if (item is int intValue)
                {
                    return Math.Abs(intValue);  // Treat integer as a positive value
                }
                else if (item is double doubleValue)
                {
                    return Math.Abs((decimal)doubleValue);  // Convert double to decimal for proper comparison
                }

                throw new InvalidOperationException("Unknown object type.");
            }

            



        }
    }
}
