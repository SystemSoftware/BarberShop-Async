using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sleeping_Barber_AsyncAwait
{
    class PPT
    {
        private void DumpWebPage(string uri)
        {
            WebClient webClient = new WebClient();
            string page = webClient.DownloadString(uri);
            Console.WriteLine(page);
        }

        private async void DumpWebPageAsync(string uri)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringTaskAsync(uri) <- magic(SecondHalf);
        }

        private void SecondHalf(string awaitedResult)
        {
            string page = awaitedResult;
            Console.WriteLine(page);
        }
    }
}
