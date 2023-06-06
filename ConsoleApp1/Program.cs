namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("datetime");
            //DateTime date = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("dayofweek");
            byte dayofweek = byte.Parse(Console.ReadLine());
            DayOfWeek day = (DayOfWeek)dayofweek;
            string dayName = Enum.GetName(typeof(DayOfWeek), day);
            Console.WriteLine(dayName);

            DateOnly dateOnly = new DateOnly();





            byte hour = byte.Parse(Console.ReadLine());

        }
    }
}