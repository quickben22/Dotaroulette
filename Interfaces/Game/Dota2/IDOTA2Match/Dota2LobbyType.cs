﻿namespace PortableSteam
{
    /// <summary>
    /// Lobby types for Dota 2. 
    /// </summary>
    public enum Dota2LobbyType
    {
        /// <summary>
        /// Invalid.
        /// </summary>
        Invalid = -1,
        /// <summary>
        /// Public match making.
        /// </summary>
        PublicMatchmaking = 0,
        /// <summary>
        /// Practise.
        /// </summary>
        Practise = 1,
        /// <summary>
        /// Tournamnet.
        /// </summary>
        Tournament = 2,
        /// <summary>
        /// Tutorial.
        /// </summary>
        Tutorial = 3,
        /// <summary>
        /// Coop with bots.
        /// </summary>
        CoopWithBots = 4,
        /// <summary>
        /// Team match.
        /// </summary>
        TeamMatch = 5,
        /// <summary>
        /// Solo queue.
        /// </summary>
        SoloQueue = 6,
         /// <summary>
        /// Ranked MMR.
        /// </summary>
        Ranked = 7,
    }
}
