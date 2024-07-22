
using Microsoft.EntityFrameworkCore;

namespace MMOGame_EFCore
{
    public class Program
    {

        public static void Main(string[] args)
        {
            DBCommands.InitializeDB(forceReset: false);
            
           
            // CRUD (Create-Read-Update-Delete)
            Console.WriteLine("명령어를 입력하세요");
            Console.WriteLine("[0] Force Reset");
            Console.WriteLine("[1] ShowItems");            
            Console.WriteLine("[2] TestUpdateAttach");            
            //Console.WriteLine("[3] Select Loading");

            
            while (true)
            {
                Console.Write("> ");
                string command  = Console.ReadLine();
                switch (command)
                {
                    case "0":
                        DBCommands.InitializeDB(forceReset: true);
                        break;
                    case "1":
                        DBCommands.ShowItems();
                        break;
                    case "2":
                        DBCommands.TestUpdateAttach();
                        break;
                    case "3":                        
                        break;
                }
            }
            
        }
    }
}
