﻿namespace P03_FootballBetting.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public int GameId { get; set; }

        public DateTime DateTime { get; set; }
        
        public int HomeTeamId { get; set; }

        public Team HomeTeam { get; set; }

        public byte HomeTeamGoals { get; set; }

        public double HomeTeamBetRate { get; set; }
        
        public int AwayTeamId { get; set; }

        public Team AwayTeam { get; set; }

        public byte AwayTeamGoals { get; set; }

        public double AwayTeamBetRate { get; set; }
        
        public double DrawBetRate { get; set; }

        public int Result { get; set; }

        public ICollection<Bet> Bets { get; set; } = new List<Bet>();

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();
    }
}
