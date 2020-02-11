﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameServices _services;
        public static IHubContext<GameHub> _hubContext;
        public static Game GameInstance = new Game();

        public GameController(IHubContext<GameHub> hubContext)
        {
            _services = new GameService();
            _hubContext = hubContext;
        }
        public static void UpdatePlayersFor(int gameId, List<Player> playerData)
        {
            List<string> keys = new List<string>();

            foreach (KeyValuePair<string, int> pair in GameHub._connections)
            {
                if (pair.Value == gameId)
                    keys.Add(pair.Key);
            }

            foreach (string key in keys)
            {
                _hubContext.Clients.Client(key).SendAsync("UpdatePlayers", playerData);
            }
        }
        public static void UpdateUnitsFor(int gameId, List<Character> unitData)
        {
            List<string> keys = new List<string>();

            foreach (KeyValuePair<string, int> pair in GameHub._connections)
            {
                if (pair.Value == gameId)
                    keys.Add(pair.Key);
            }

            foreach (string key in keys)
            {
                _hubContext.Clients.Client(key).SendAsync("UpdateUnits", unitData);
            }
        }

        public static void EndGame(int gameId, int side)
        {
            List<string> keys = new List<string>();

            foreach (KeyValuePair<string, int> pair in GameHub._connections)
            {
                if (pair.Value == gameId)
                    keys.Add(pair.Key);
            }

            foreach (string key in keys)
            {
                _hubContext.Clients.Client(key).SendAsync("EndGame", side);
            }
        }

        // Old HttpGet methods...
        /*
        [HttpGet("getUnits")]
        public ActionResult<Character[]> GetUnits()
        {
            var units = _services.GetUnits();

            if (units == null)
                return new Character[0];

            return units.ToArray();
        }

        [HttpGet("getPlayers")]
        public ActionResult<Player[]> GetPlayers()
        {
            var players = _services.GetPlayers();

            if (players == null || players[0] == null || players[1] == null)
                return NotFound();

            return players;
        }*/

        [HttpGet("init/{team=white}/{id=1}")]
        public ActionResult<bool> Init(string team, int id)
        {
            _services.Init(id, team);
            return true;
        }

        [HttpGet("play/{id=1}")]
        public ActionResult<bool> Play(int id)
        {
            _services.Play(id);
            return true;
        }
        [HttpGet("end/{id=1}")]
        public ActionResult<bool> End(int id)
        {
            _services.End(id);
            return true;
        }

        [HttpGet("buy/{id=1}/{player=1}/{unit=doggo}")]
        public ActionResult<bool> Buy(int id, int player, string unit)
        {
            _services.Buy(id, player, unit);
            return true;
        }

        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "game test";
        }
    }
}