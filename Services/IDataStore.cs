public interface IDataStore
{
    List<Person> getPersonList(int id, int limit);
    Person? getPerson(int id);
    string getValue(Person p, string name);
    bool createPerson(Person p);
    bool updatePerson(Person p);
    bool deletePerson(Person p);
    List<String> getProperties();
}