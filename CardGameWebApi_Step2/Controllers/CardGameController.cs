﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CardGameWebApi.Models;
using System.Security.AccessControl;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods
//https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-6.0
//https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.httpgetattribute?view=aspnetcore-6.0
//https://docs.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=net-6.0
//https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-6.0#iactionresult-type
//https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok?view=aspnetcore-6.0


namespace CardGameWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]  //All actions (methods) in the controller will be named as the method
                                          //allows me to use simple   [HttpGet]
                                          
    public class CardGameController : ControllerBase
    {
        private ILogger<CardGameController> _logger;

        static GameStatus gameStatus = new GameStatus();

        //GET: api/startgame?gametype={gameType}
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GameStatus))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> StartGame(string gameType)
        {
            _logger.LogInformation($"Action: {nameof(CardGameController)}.{nameof(StartGame)} executed");

            if (gameStatus.StartGame(gameType.ToLower().Trim()))
            {
                return Ok(gameStatus);
            }

            return BadRequest($"Game of type {gameStatus.GameType} is already running");
        }

        
        //GET: api/endgame
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GameStatus))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EndGame()
        {
            _logger.LogInformation($"Action: {nameof(CardGameController)}.{nameof(EndGame)} executed");

            if (gameStatus.EndGame())
            {
                return Ok(gameStatus);
            }

            return BadRequest($"No game is running");
        }

        
        //GET: api/dealcard
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PlayingCard))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DealCard()
        {
            _logger.LogInformation($"Action: {nameof(CardGameController)}.{nameof(DealCard)} executed");

            if (!gameStatus.IsRunning)
                return BadRequest($"No game is running");

            return Ok(PlayingCard.CreateRandom());
        }

        
        //GET: api/dealcards/?nrOfCard={nrOfCards}
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayingCard>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DealCards(string nrOfCards)
        {
            _logger.LogInformation($"Action: {nameof(CardGameController)}.{nameof(DealCards)} executed");

            if (!gameStatus.IsRunning)
                return BadRequest($"No game is running");

            int _nrOfCards;
            if (int.TryParse(nrOfCards, out _nrOfCards) && _nrOfCards > 1 && _nrOfCards <= 5)
            {
                //deal the cards
                var hand = new List<PlayingCard>();
                for (int i = 0; i < _nrOfCards; i++)
                {
                    hand.Add(PlayingCard.CreateRandom());
                }

                return Ok(hand);
            }

            //indicate error in nrOfCards
            return BadRequest($"{nrOfCards} is not a valid amount of cards");
        }


        //POST: api/winningcard   
        //Body: List<PlayingCard> in Json
        [HttpPost]                          //Needs to be PUT or POST as I have a request Body
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlayingCard>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> WinningCards([FromBody] List<PlayingCard> hand)
        {
            _logger.LogInformation($"Action: {nameof(CardGameController)}.{nameof(WinningCards)} executed");

            if (!gameStatus.IsRunning)
                return BadRequest($"No game is running");

            if (hand.Count <1)
                return BadRequest($"No cards to evaluate");

            //respond with winning card as part of a possible winning hand
            var response = new List<PlayingCard>();
            response.Add(hand[0]);
            return Ok(response);
        }

        public CardGameController(ILogger<CardGameController> logger)
        {
            _logger = logger;
            _logger.LogInformation($"CardGameController started");
        }
    }
}

