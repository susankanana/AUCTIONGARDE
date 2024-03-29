﻿namespace AUCTIONGARDE_Frontend.Models.Arts.Dtos
{
    public class AddArtDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime ExpiryTime { get; set; }
        public string Status { get; set; } = "True";
        //public bool newArt { get; set; } = true;
        public int StartPrice { get; set; }
        public string ArtImage { get; set; }
        public string Category { get; set; }
    }
}
