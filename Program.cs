using RestSharp;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ClientLoadBalancer
{
    class Program
    {
        private static string loadBalancerUrl = "https://sunapiserver.herokuapp.com";
        private static InputDto input = new InputDto{ Latitude = 55.85463, Longitude = 8.56321, Date = "08/05/2022"};

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            run(10);
            Console.ReadLine();
        }

        public static void run(int loops)
        {
            while(loops > 0)
            {
                Task.Run(() => requestSunrise(loops));
                Thread.Sleep(10);
                loops--;
            }
        }

        public static void requestSunset(int num)
        {
            RestClient c = new RestClient(loadBalancerUrl + "/sun/sunset");
            var request = new RestRequest();
            request.AddJsonBody(input);
            var response = c.GetAsync<OutputDto>(request);
            response.Wait();
            OutputDto outp = response.Result;

        }

        public static void requestSunrise(int num)
        {
            string initTS = DateTime.Now.ToString();

            RestClient c = new RestClient(loadBalancerUrl + "/sun/sunrise");
            var request = new RestRequest();
            request.AddJsonBody(input);
            var response = c.GetAsync<OutputDto>(request);
            response.Wait();
            OutputDto outp = response.Result;

            string endTS = DateTime.Now.ToString();

            Console.WriteLine("###############################\n" +
                              "The request num: {0}\n " +
                              "was executed on machine: {1}\n " +
                              "Start execution time: {2}\n " +
                              "End execution time: {3}\n " +
                              "Result: {4}",num,outp.Machine,initTS,endTS, outp.ToString());
        }
    }    
}
