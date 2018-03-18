using LiteDB;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemEnvironment = System.Environment;

namespace HearMeApp.Android
{
    public class DatabaseProvider
    {
        private const string FILE_NAME = "local.db";

        private readonly string ConnectionString;

        public DatabaseProvider()
        {
            var folder = SystemEnvironment.GetFolderPath(SystemEnvironment.SpecialFolder.Personal);
            ConnectionString = Path.Combine(folder, FILE_NAME);
        }

        public List<Contact> GetAll()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var contacts = db.GetCollection<Contact>();
                return contacts.FindAll().ToList();
            }
        }

        public void Add(Contact contact)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var contacts = db.GetCollection<Contact>();
                contacts.Insert(contact);
            }
        }

        public bool Remove(Contact contact)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var contacts = db.GetCollection<Contact>();
                var removedFilesCount = contacts.Delete(Query.EQ(nameof(contact.Name), contact.Name));

                return removedFilesCount > 0;
            }   
        }
    }
}