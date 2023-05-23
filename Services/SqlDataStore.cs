using demo.Models;

public class SqlDataStore : IDataStore
{
    private readonly ILogger<SqlDataStore> logger;
    IfiStDbContext ctx;

    public SqlDataStore(ILogger<SqlDataStore> logger, IfiStDbContext ctx)
    {
        this.logger = logger;
        this.ctx = ctx;
    }
    public bool createPerson(Person p)
    {
        ctx.Persons.Add(p);
        ctx.SaveChanges();
        return true;
    }

    public bool deletePerson(Person p)
    {
        ctx.Persons.Remove(p);
        ctx.SaveChanges();
        return true;
    }

    public Person? getPerson(int id)
    {
        return ctx.Persons.Where(p=>p.Id==id).First<Person>();
    }

    public List<Person> getPersonList(int id, int limit)
    {
        return ctx.Persons.ToList();
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

    public bool updatePerson(Person p)
    {
        Person q = ctx.Persons.Where(q=>q.Id==p.Id).First<Person>();
        q.FirstName = p.FirstName;
        q.LastName = p.LastName;
        q.Phone = p.Phone;
        ctx.SaveChanges();
        return true;
    }
}