using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Shared.LaunchManager;

public class LaunchManager : ILaunchManager
{
    
    public readonly LaunchManagerOptions Options;
    
    public LaunchManager(LaunchManagerOptions options)
    {
        Options = options;
    }

    public void OnStart()
    {
        ExpectAddress();
    }

    private void ExpectAddress() 
    {
        if (String.IsNullOrEmpty(Options.ExpectedAddress))
            return;

        int count = 20;
        int delay = 1000;

        using var client = new HttpClient();

        do
        {
            try
            {
                client.GetAsync(Options.ExpectedAddress).Wait();
            }
            catch
            {
                Thread.Sleep(delay);
            }
        } while (count-- > 0);
    }

}
