namespace Fuel_Prices
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Потребителят въвежда година
            Console.WriteLine("Въведи година:");
            int year = int.Parse(Console.ReadLine());

            // Масив с цените на горивата за всеки месец
            double[] fuelPrices = new double[12];
            // Масив с имената на месеците на български
            string[] months = { "Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември" };

            // Потребителят въвежда цените на горивата за всеки месец от годината
            for (int i = 0; i < 12; i++)
            {
                Console.Write($"{months[i]} {year}: ");
                fuelPrices[i] = double.Parse(Console.ReadLine());
            }

            // Променливи за най-високата, най-ниската и средната цена на горивата
            double maxPrice = fuelPrices[0];
            double minPrice = fuelPrices[0];
            double sum = 0;

            // Намиране на най-високата, най-ниската и сумата от всички цени на горивата
            for (int i = 0; i < 12; i++)
            {
                sum += fuelPrices[i];
                if (fuelPrices[i] > maxPrice)
                    maxPrice = fuelPrices[i];
                if (fuelPrices[i] < minPrice)
                    minPrice = fuelPrices[i];
            }

            // Пресмятане на средната цена на горивата за годината
            double averagePrice = sum / 12;

            // Извеждане на резултатите за най-високата, най-ниската и средната цена на горивата
            Console.WriteLine($"Месецът с най-висока цена на горивото е: {months[Array.IndexOf(fuelPrices, maxPrice)]} със цена {maxPrice}");
            Console.WriteLine($"Месецът с най-ниска цена на горивата е: {months[Array.IndexOf(fuelPrices, minPrice)]} със цена {minPrice}");
            Console.WriteLine($"Средната цена на горивата за годината е: {averagePrice}");

            // Записване на цените на горивата във файл
            using (StreamWriter writer = new StreamWriter($"FuelPrices_{year}.txt"))
            {
                for (int i = 0; i < 12; i++)
                {
                    writer.WriteLine($"{months[i]}: {fuelPrices[i]}");
                }
            }

            // Извеждане на съобщение за успешно записване на цените във файл
            Console.WriteLine("Цените на горивата са записани във файл.");
        }
    }
}
