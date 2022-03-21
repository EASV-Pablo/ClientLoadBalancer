using RestSharp;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ClientLoadBalancer
{
    class Program
    {
        private static string loadBalancerUrl = "http://localhost:5100";
        private static InputDto input = new InputDto{ Latitude = 55.85463, Longitude = 8.56321, Date = "08/05/2022"};

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            run(50);
            Console.ReadLine();
        }

        public static void run(int loops)
        {
            Random rnd = new Random();
            while (loops > 0)
            {
                if(rnd.Next(0,2) % 2 == 0 )
                    Task.Run(() => requestSunrise());
                else
                    Task.Run(() => requestSunset());

                Thread.Sleep(rnd.Next(10,50));
                loops--;
            }
        }

        public static void requestSunset()
        {
            string initTS = DateTime.Now.ToString();

            RestClient c = new RestClient(loadBalancerUrl + "/sun/sunset");
            var request = new RestRequest();
            request.AddJsonBody(input);
            var response = c.GetAsync<OutputDto>(request);
            response.Wait();
            OutputDto outp = response.Result;

            string endTS = DateTime.Now.ToString();

            print(outp, initTS, endTS);

        }

        public static void requestSunrise()
        {
            string initTS = DateTime.Now.ToString();

            RestClient c = new RestClient(loadBalancerUrl + "/sun/sunrise");
            var request = new RestRequest();
            request.AddJsonBody(input);
            var response = c.GetAsync<OutputDto>(request);
            response.Wait();
            OutputDto outp = response.Result;

            string endTS = DateTime.Now.ToString();

            print(outp, initTS, endTS);
        }

        public static void print(OutputDto outp, string initTS, string endTS)
        {
            Console.WriteLine("###############################\n" +
                              "The request was executed on machine: {0}\n " +
                              "Start execution time: {1}\n " +
                              "End execution time: {2}\n "
                              , outp.Machine, initTS, endTS);
        }
    }    
}
