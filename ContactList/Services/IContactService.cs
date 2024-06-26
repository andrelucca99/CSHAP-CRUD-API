namespace ContactList.Services;
using ContactList.Models;

public interface IContactService
{
  Person addPerson(Person person);
  Person[] getPersonList();
  Person getByIdPerson(int PersonId);
  Person updatePerson(int PersonId, Person person);
  void deletePerson(int PersonId);
}