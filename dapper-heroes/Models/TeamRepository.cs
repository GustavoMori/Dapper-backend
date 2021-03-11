using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace dapper_heroes.Models
{
    public class TeamRepository
    {
        private string connectionString;
        public TeamRepository()
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
        public Team Add(Team team)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string nameAlreadyExistTable = @"SELECT * from tb_teams WHERE NAME LIKE @name";
                dbConnection.Open();
                var nameAlreadyExist = dbConnection.Query<Team>(nameAlreadyExistTable, new { name = team.name }).SingleOrDefault();
                dbConnection.Close();

                if (nameAlreadyExist == null)
                {
                    string sQuery = @"DECLARE @id_team INT
                        insert into tb_teams (name) values (@name);
                        SELECT @id_team = SCOPE_IDENTITY();
                        SELECT * FROM tb_teams WHERE id_team = @id_team;";
                    dbConnection.Open();
                    var newTeam = dbConnection.Query<Team>(sQuery, new { name = team.name }).SingleOrDefault();
                    return newTeam;
                }

                else
                {
                    throw new Exception("That name is already in use");
                };


            }
        }
        #endregion

        #region GETS
        public IEnumerable<Team> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM tb_teams";
                dbConnection.Open();
                return dbConnection.Query<Team>(sQuery);
            }
        }
        public Team GetById(int id_team)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM tb_teams WHERE id_team=@Id_team";
                dbConnection.Open();
                return dbConnection.Query<Team>(sQuery, new { Id_team = id_team }).FirstOrDefault();
            }
        }
        public IEnumerable<Hero> GetHeroByRelationship(int id_team)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT 
                                    h.*
                                    FROM tb_teams_heroes AS g
                                    INNER JOIN tb_heroes AS h ON g.hero_idfk = h.id
                                    INNER JOIN tb_teams AS t ON g.team_idfK = t.id_team
                                    WHERE t.id_team = @id_team ;";
                dbConnection.Open();
                return dbConnection.Query<Hero>(sQuery, new { id_team });
            }
        }
        #endregion


        #region DELETE
        public void Delete(int id_team)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"DELETE FROM tb_teams_heroes WHERE team_idfk=@Id_team
                                  DELETE FROM tb_teams WHERE id_team=@Id_team";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id_team = id_team });
            }
        }
        #endregion

        #region UPDATE
        public void Update(Team team)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"UPDATE tb_teams SET name=@name WHERE id_team=@id_team";
                dbConnection.Open();
                dbConnection.Execute(sQuery, team);
            }
        }
        #endregion
    }
}
