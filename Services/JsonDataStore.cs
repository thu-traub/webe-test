using System.Text.Json;

class JsonDataStore : IDataStore
{
    private readonly ILogger<JsonDataStore> logger;
    private readonly IWebHostEnvironment henv;
    private readonly IConfiguration conf;
    private string DataFile;
    private List<Person> MemoryDB;

    public JsonDataStore(ILogger<JsonDataStore> logger, IWebHostEnvironment henv, IConfiguration conf)
    {
        this.logger = logger;
        this.henv = henv;
        this.conf = conf;
        string? dfn = conf.GetValue<string>("JsonDataStore:DataFile");
        if (String.IsNullOrEmpty(dfn)) {
            logger.LogError("No data file provided");
            DataFile = "";
        } else {
            DataFile = Path.Combine(henv.ContentRootPath, dfn);
        }
        MemoryDB = loadData();
    }

    private List<Person> loadData()
    {
        if (DataFile == "") return new();
        if (!File.Exists(DataFile)) {
            return new();
        }
        string data = File.ReadAllText(DataFile);
        return JsonSerializer.Deserialize<List<Person>>(data)??new();
    }

    private void saveData()
    {
        JsonSerializerOptions opt = new();
        opt.WriteIndented = true;
        File.WriteAllText(DataFile, JsonSerializer.Serialize<List<Person>>(MemoryDB, opt));
    }

    public bool createPerson(Person p)
    {
        if (MemoryDB.Count() == 0) {
            p.Id = 0;
        } else {
            p.Id = MemoryDB.Max(p=>p.Id)+1;
        }
        MemoryDB.Add(p);
        saveData();
        return true;
    }

    public bool deletePerson(Person p)
    {
        Person q = MemoryDB.First<Person>(q=>q.Id == p.Id);
        MemoryDB.Remove(q);
        saveData();
        return true;
    }

    public Person? getPerson(int id)
    {
        return MemoryDB.First<Person>(p=>p.Id==id);
    }

    public List<Person> getPersonList(int id, int limit)
    {
        return (MemoryDB??new()).OrderBy(p=>p.Id).ToList<Person>();
    }

    public bool updatePerson(Person p)
    {
        Person q = MemoryDB.First<Person>(q=>q.Id == p.Id);
        if (q != null) MemoryDB.Remove(q);
        MemoryDB.Add(p);
        saveData();
        return true;
    }

    public List<string> getProperties()
    {
        List<string> P = new();
        foreach (var i in typeof(Person).GetProperties())
        {
            if (!i.GetMethod?.IsStatic??true) P.Add(i.Name);
        }
        return P;
    }

    public string getValue(Person p, string name)
    {
        return ((p.GetType().GetProperty(name)?.GetValue(p)??"") as string)??"";
    }
}