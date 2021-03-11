using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_heroes.Models
{
    public class HeroRepository
    {
        private string connectionString;
        public HeroRepository()
        {
            connectionString = @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=heroes;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
      
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }


        #region POST
        public Hero Add(Hero hero)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string nameAlreadyExistTable = @"SELECT * from tb_heroes WHERE NAME LIKE @name";
                dbConnection.Open();
                var nameAlreadyExist = dbConnection.Query<Hero>(nameAlreadyExistTable, new {name = hero.name}).SingleOrDefault();
                dbConnection.Close();

                if (nameAlreadyExist == null)
                {
                    string sQuery = @"DECLARE @id INT
                        insert into tb_heroes (name,power,agility) values (@name,@power,@agility);
                        SELECT @id = SCOPE_IDENTITY();
                        SELECT * FROM tb_heroes WHERE id = @id;";
                    dbConnection.Open();
                    var newHero = dbConnection.Query<Hero>(sQuery, new { name = hero.name, power = hero.power, agility = hero.agility }).SingleOrDefault();
                    return newHero;
                }

                else
                {
                    throw new Exception("That name is already in use");
                };
            }
        }

        #endregion

        #region GETS
        public IEnumerable<Hero> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM tb_heroes";
                dbConnection.Open();
                return dbConnection.Query<Hero>(sQuery);
            }
        }
        public Hero GetPowerfull()
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT * FROM tb_heroes WHERE hero_power = (SELECT TOP 1 MAX(hero_power) FROM tb_heroes)";
                string sQuery = @"SELECT TOP 1 * FROM tb_heroes ORDER BY power DESC";
                dbConnection.Open();
                return dbConnection.Query<Hero>(sQuery).FirstOrDefault();
            }
        }
        public Hero GetById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM tb_heroes WHERE id=@Id";
                dbConnection.Open();
                return dbConnection.Query<Hero>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }
        #endregion

        #region DELETE
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"DELETE FROM tb_heroes WHERE id=@Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        #endregion

        #region UPDATE
        public void Update(Hero hero)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"UPDATE tb_heroes SET name=@name, power=@power, agility=@agility WHERE id=@id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, hero);
            }
        }
        #endregion
    }
}
