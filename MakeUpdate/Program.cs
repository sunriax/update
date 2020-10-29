using System;
using RaGae.UpdateLib;
using RaGae.UpdateLib.UpdateModelLib;

namespace MakeUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Update update = new Update(args);
                update.UpdateMessage += Console.WriteLine;
                update.ExecuteUpdate();
            }
            catch (BaseUpdateException ex)
            {
                Console.WriteLine(ex.ErrorMessage());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
