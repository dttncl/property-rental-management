namespace property_rental_management.Controllers
{
    public class RandomIDGenerator
    {
        private static Random random = new Random();

        public static string GenerateRandomID(String prefix, int numOfDigits)
        {
            int randomNumber;

            switch (numOfDigits)
            {
                case 3:
                    randomNumber = random.Next(100, 1000); // 3-digits
                    break;
                case 4:
                    randomNumber = random.Next(1000, 10000); // 4-digits
                    break;
                case 7:
                    randomNumber = random.Next(1000000, 10000000); // 7-digits
                    break;
                case 9:
                    randomNumber = random.Next(100000000, 1000000000); // 9-digits
                    break;
                default:
                    randomNumber = random.Next(0, 10); // 9-digits
                    break;
            }

            return prefix + randomNumber.ToString();
        }

        public static int GenerateRandomID(int numOfDigits)
        {
            int randomNumber;

            switch (numOfDigits)
            {
                case 3:
                    randomNumber = random.Next(100, 1000); // 3-digits
                    break;
                case 4:
                    randomNumber = random.Next(1000, 10000); // 4-digits
                    break;
                case 7:
                    randomNumber = random.Next(1000000, 10000000); // 7-digits
                    break;
                case 9:
                    randomNumber = random.Next(100000000, 1000000000); // 9-digits
                    break;
                default:
                    randomNumber = random.Next(0, 10); // 9-digits
                    break;
            }

            return randomNumber;
        }
    }
}
