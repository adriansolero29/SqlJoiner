namespace SqlJoiner.View.ConsoleUI
{
    internal class Program
    {
        public Program()
        {
                
        }

        public async Task Start()
        {
            Console.WriteLine("===== SQL GENERATOR =====");
            Console.Write("""
                Menu:
                1. Load Schemas 2. Exit

                """);

            string? menuResult = Console.ReadLine();

            int selectedMenu = 0;
            do
            {
                Console.Write("Invalid result. Please try again: ");
                menuResult = Console.ReadLine();
            }
            while ((int.Parse(menuResult ?? "0") != 1 || (int.Parse(menuResult ?? "0") != 2) && int.TryParse(menuResult, out selectedMenu) == false));

            Console.Clear();
            Console.WriteLine("Loading Schemas...");




            Console.ReadLine();
        }

        async static void Main(string[] args)
        {
            Program p = new Program();
            await p.Start();
        }
    }
}
