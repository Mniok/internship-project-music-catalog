﻿namespace musicCatalogDotNetAPI.Models
{
    public class Link
    {
        public int LinkId { get; set; }

        public string ToSite { get; set; }      // ex. https://blindguardian.bandcamp.com/album/follow-the-blind  ==> bandcamp      (pole bandcamp link: ______
                                                // (aczkolwiek tu jest blindguardian.bandcamp subdomena to dodatkowo pole Subdomain??? albo bez bandcampa???
                                                // ale cel jest spotify, itunes, soundcloud, i tyle powinno starczyć)

        public string LinkBody { get; set; }    // ex. https://blindguardian.bandcamp.com/album/follow-the-blind  ==> follow-the-blind

                                                //also w sumie ma być do utworów nie albumów więc bardziej
                                                // https://blindguardian.bandcamp.com/track/fast-to-madness  ==>  fast-to-madness




        public int SongId { get; set; }

        public Song Song { get; set; }


    }
}
