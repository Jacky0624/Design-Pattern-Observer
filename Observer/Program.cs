
MachineDownTimeStation station = new MachineDownTimeStation();
IAlerter alerter = new MailManager();
IObserver SW = new Department(alerter, "SW");
IObserver QE = new Department(alerter, "QE");
IObserver ED = new Department(alerter, "ED");
station.Attach(SW);
station.Attach(QE);
station.Attach(ED);

station.StartTime = DateTime.Now;

interface IObserver
{
    void Update(ISubject subject);
}

interface ISubject
{
    void Attach(IObserver observer);
    void Notify();
}

class MachineDownTimeStation : ISubject
{
    private List<IObserver> _departments;

    public MachineDownTimeStation()
    {
        _departments = new List<IObserver>();
    }

    private DateTime _startTime;
    public DateTime StartTime 
    {
        get => _startTime;
        set
        {
            if(_startTime != value)
            {
                _startTime = value;
                Notify();
            }
        }
    }

 
    public void Attach(IObserver department)
    {
        _departments.Add(department);
    }

    public void Notify()
    {
        foreach (var department in _departments)
        {
            department.Update(this);
        }
    }
}
class Department : IObserver
{
    private readonly IAlerter _alerter;
    public string Name { get; }
    public Department(IAlerter alerter, string name)
    {
        _alerter = alerter;
        Name = name;
    }
    public void Update(ISubject subject)
    {
        var station = subject as MachineDownTimeStation;

        _alerter.Alert(Name, $"Machine is down from {station?.StartTime.ToString("yyyy/MM/dd HH:mm:ss")}");
    }

  
}

interface IAlerter
{
    void Alert(string department, string message);
}

class MailManager: IAlerter
{
    public void Alert(string department ,string message)
    {
        Console.WriteLine($"send mail to {department}, content : {message}");

    }
}




