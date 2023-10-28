using System.Diagnostics;

RescueFromRiot rescueTeam = new();
rescueTeam.DeleteRiotEvent += (isFree, proccessKillerResponse) =>
{
    if (isFree)
        global::System.Console.WriteLine($"Özgürsünüz... / {proccessKillerResponse}");
    else
        global::System.Console.WriteLine($"Özgürleştirilemediniz... / {proccessKillerResponse}");
};

await rescueTeam.ActivateRescueFromRiot();

class RescueFromRiot
{
    public delegate void RescueHandler(bool isFree, string proccessKillerResponse);
    public event RescueHandler DeleteRiotEvent;

    private readonly string[] pathsToClean =
    {
        @"C:\Riot Games",
        @"C:\ProgramData\Riot Games",
        @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Riot Games",
        @"C:\Users\kocak\AppData\Local\Riot Games",
        @"C:\Users\kocak\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Riot Games",
    };

    public async Task ActivateRescueFromRiot()
    {
        while (true)
        {
            await Task.Delay(1000);
            await Task.Run(() =>
            {
                foreach (string directory in pathsToClean)
                {
                    if (Directory.Exists(directory))
                    {
                        try
                        {
                            string proccessKillResult = TerminateProcessesUsingPath(directory);
                            Directory.Delete(directory, true);
                            DeleteRiotEvent(true, proccessKillResult);
                        }
                        catch (Exception e)
                        {
                            DeleteRiotEvent(false, "");
                            Console.WriteLine($"Riot saldiridan kurtuldu, tekrar deneniyor. {directory}: {e.Message}");
                        }
                    }
                }
            });
        }
    }
    private string TerminateProcessesUsingPath(string path)
    {
        var allProcesses = Process.GetProcesses();

        foreach (var process in allProcesses)
        {
            try
            {
                if (process.Modules.Cast<ProcessModule>().Any(module => module.FileName.StartsWith(path)))
                {
                    process.Kill();
                    process.WaitForExit();
                    return "Riot Client Öldürüldü";
                }
                else
                {
                    return "İşlemler taranıyor.";
                }
            }
            catch
            {
                return "İşlemler taranıyor.";
            }
        }
        return "İşlemler Tarandı";
    }
}
