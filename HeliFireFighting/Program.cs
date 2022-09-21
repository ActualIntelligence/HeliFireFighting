using System;

namespace HeliFireFighting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var app = new Main())
                app.Run();
        }
    }
}