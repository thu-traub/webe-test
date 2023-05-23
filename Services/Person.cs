public class Person
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }

    //public static Person Empty { get; } = new Person();

    public override string ToString()
    {
        return $"{Id}: {LastName}, {FirstName} - {Phone}";
    }
}