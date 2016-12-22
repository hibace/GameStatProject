﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Dota2Stats.Models;
using Dota2Stats.Middleware;
using Npgsql;
namespace Dota2Stats.Controllers
{
    using Middleware;
    public class playerController : ApiController
    {
        // GET api/player?name=hib
        public Player GetPlayerByName(string name)
        {
            Player player = new Player();
            NpgsqlHelper.Connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = NpgsqlHelper.Connection;
                cmd.Parameters.Add(new NpgsqlParameter("@name", name));
                cmd.CommandText = "SELECT * FROM player WHERE nickname = @name";
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player = new Player
                            {
                                id = reader.GetInt32(0),
                                nickname = reader.GetString(1),
                                number_of_games = reader.GetInt32(2)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            NpgsqlHelper.Connection.Close();
            return player;
        }

        // GET api/player?number_of_games_greater_than=100
        public Player GetPlayerByNumberOfGames(int number_of_games_greater_than)
        {
            Player player = new Player();
            NpgsqlHelper.Connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = NpgsqlHelper.Connection;
                cmd.Parameters.Add(new NpgsqlParameter("@number_of_games_greater_than", number_of_games_greater_than));
                cmd.CommandText = "SELECT * FROM player WHERE number_of_games >= @number_of_games_greater_than";
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player = new Player
                            {
                                id = reader.GetInt32(0),
                                nickname = reader.GetString(1),
                                number_of_games = reader.GetInt32(2)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            NpgsqlHelper.Connection.Close();
            return player;
        }

        // GET api/player?id_match=1
        public IEnumerable<Player> GetPlayers(int id_match)
        {
            var playerlist = new List<Player>();
            try
            {
                NpgsqlHelper.Connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = NpgsqlHelper.Connection;
                    command.Parameters.Add(new NpgsqlParameter("@id_match", id_match));
                    command.CommandText = "SELECT player.id, player.nickname from player, maintemp WHERE player.id = maintemp.id_player AND maintemp.id_match = @id_match";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            playerlist.Add(new Player
                            {
                                id = reader.GetInt32(0),
                                nickname = reader.GetString(1)
                                });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                
            }
            NpgsqlHelper.Connection.Close();
            return playerlist;
        }




        // GET api/player
        public IEnumerable<Player> Get()
        {
            var player = new List<Player>();
            NpgsqlHelper.Connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = NpgsqlHelper.Connection;
                cmd.CommandText = "SELECT * FROM player ORDER by id ASC";
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player.Add(new Player
                            {
                                id = reader.GetInt32(0),
                                nickname = reader.GetString(1),
                                number_of_games = reader.GetInt32(2)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                NpgsqlHelper.Connection.Close();
            }
            NpgsqlHelper.Connection.Close();
            return player;
        }

        // GET api/player/5
        public Player Get(int id)
        {
            Player player = new Player();
            NpgsqlHelper.Connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = NpgsqlHelper.Connection;
                cmd.CommandText = "SELECT * FROM player WHERE id = @id ORDER by id ASC";
                cmd.Parameters.Add(new NpgsqlParameter("@id", id));
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            player = new Player
                            {
                                id = reader.GetInt32(0),
                                nickname = reader.GetString(1),
                                number_of_games = reader.GetInt32(2)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            NpgsqlHelper.Connection.Close();
            return player;
        }

        // POST api/player
        public Player Post([FromBody]Player value)
        {
            Player insertedPlayer = new Player();
            NpgsqlHelper.Connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = NpgsqlHelper.Connection;
                cmd.CommandText = "INSERT INTO player (nickname, number_of_games) VALUES (@nickname, @number_of_games)";
                //cmd.Parameters.Add(new NpgsqlParameter("@id", value.id));
                cmd.Parameters.Add(new NpgsqlParameter("@nickname", value.nickname));
                cmd.Parameters.Add(new NpgsqlParameter("@number_of_games", value.number_of_games));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteReader();

                try
                {
                    NpgsqlCommand cmd2 = new NpgsqlCommand();
                    cmd2.Connection = NpgsqlHelper.Connection;
                    cmd2.CommandText = "SELECT * FROM player ORDER BY id DESC LIMIT 1";
                    try
                    {
                        using (var reader = cmd2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                insertedPlayer = new Player
                                {
                                    id = reader.GetInt32(0),
                                    nickname = reader.GetString(1),
                                    number_of_games = reader.GetInt32(2)
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    cmd2.ExecuteNonQuery();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Dispose();
                }
                catch (Exception ex)
                {

                } 
            }
            NpgsqlHelper.Connection.Close();
            return insertedPlayer;
        }

        // PUT api/player/5
        public Player Put(int id, [FromBody]Player value)
        {
            Player updatedPlayer = new Player();
            NpgsqlHelper.Connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = NpgsqlHelper.Connection;
                cmd.CommandText = "UPDATE player SET nickname=@nickname, number_of_games=@number_of_games WHERE id=@id";
                cmd.Parameters.Add(new NpgsqlParameter("@nickname", value.nickname));
                cmd.Parameters.Add(new NpgsqlParameter("@verifienumber_of_gamesd", value.number_of_games));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteReader();

                try
                {
                    NpgsqlCommand cmd2 = new NpgsqlCommand();
                    cmd2.Connection = NpgsqlHelper.Connection;
                    cmd2.CommandText = "SELECT * FROM player WHERE id = @id";
                    cmd2.Parameters.Add(new NpgsqlParameter("@id", id));
                    try
                    {
                        using (var reader = cmd2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                updatedPlayer = new Player
                                {
                                    id = reader.GetInt32(0),
                                    nickname = reader.GetString(1),
                                    number_of_games = reader.GetInt32(2)
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    cmd2.ExecuteNonQuery();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Dispose();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            NpgsqlHelper.Connection.Close();
            return updatedPlayer;
        }

        // DELETE api/player/5
        public void Delete(int id)
        {
            NpgsqlHelper.Connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                try
                {
                    cmd.Connection = NpgsqlHelper.Connection;
                    cmd.CommandText = "DELETE FROM player WHERE id=@id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            NpgsqlHelper.Connection.Close();
        }
    }
}
