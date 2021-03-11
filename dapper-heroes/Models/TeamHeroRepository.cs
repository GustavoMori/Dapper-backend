using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace dapper_heroes.Models
{
    public class TeamHeroRepository
    {
        private string connectionString;
        public TeamHeroRepository()
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

        //nao funcionando
        #region POST
        public TeamHero Add(TeamHero teamHero)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //talvez vai dar pau nessa \/ linha pq não é por WHERE NAME q se filtra
                string nameAlreadyExistTable = @"SELECT * from tb_teams_heroes WHERE team_idfk=@team_idfk";
                dbConnection.Open();
                var nameAlreadyExist = dbConnection.Query<TeamHero>(nameAlreadyExistTable, new { team_idfk = teamHero.team_idfk }).SingleOrDefault();
                dbConnection.Close();

                if (nameAlreadyExist == null)
                {
                    string sQuery = @"DECLARE @team_idfk INT
                        insert into tb_teams_heroes (team_idfk) values (@team_idfk);
                        SELECT @team_idfk = SCOPE_IDENTITY();
                        SELECT * FROM tb_teams WHERE team_idfk = @team_idfk;";
                    dbConnection.Open();
                    var newTeamHeroRelationship = dbConnection.Query<TeamHero>(sQuery, new { team_idfk = teamHero.team_idfk, hero_idfk = teamHero.hero_idfk }).SingleOrDefault();
                    return newTeamHeroRelationship;
                }

                else
                {
                    throw new Exception("That name is already in use");
                };
            }
        }
        #endregion

        //funcionando
        #region GETS
            
        public IEnumerable<TeamHero> GetAllRelationship()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM tb_teams_heroes";
                dbConnection.Open();
                return dbConnection.Query<TeamHero>(sQuery);
            }
        }

        public TeamHero GetByTeamIdFk(int team_idfk)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM tb_teams_heroes WHERE team_idfk=@Team_idfk";
                dbConnection.Open();
                return dbConnection.Query<TeamHero>(sQuery, new { Team_idfk = team_idfk }).FirstOrDefault();
            }
        }
        #endregion

        //nao ta funcionando direito
        #region UPDATE
        public void Update(TeamHero teamHero)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"UPDATE tb_teams_heroes SET team_idfk=@team_idfk WHERE hero_idfk=@hero_idfk";
                dbConnection.Open();
                dbConnection.Execute(sQuery, teamHero);
            }
        }
        #endregion

        //funcionando
        #region DELETE
        public void DeleteRelationship(int hero_idfk)
        {
            using (IDbConnection dbConnection = Connection)
            {

                string sQuery = @"DELETE FROM tb_teams_heroes WHERE hero_idfk=@Hero_idfk";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { hero_idfk = hero_idfk });
            }
        }
        #endregion
    }
}
