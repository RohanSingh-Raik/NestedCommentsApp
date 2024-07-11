class ConsoleUserInteractor : IUserInteractor
{
    public string? ReadFromUser()
    {
        string? data = Console.ReadLine();
        return data;
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }
}