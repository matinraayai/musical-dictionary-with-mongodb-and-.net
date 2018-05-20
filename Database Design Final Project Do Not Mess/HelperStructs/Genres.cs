using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs
{
    //This enum class was created based on the specs of discogs.com
    public sealed class Genre
    {
        private Genre() { }
        public readonly static string[] genres = { "Blues", "Brass & Military", "Children's", "Classical", "Electronic", "Folk, World, & Country", "Funk / Soul", "Hip-Hop", "Jazz", "Latin", "Non-Music",
        "Pop", "Reggae", "Rock", "Stage & Screen" };
        public enum genreNumber { BLUES, BRASS, CHILDREN, CLASSICAL, ELECTRONIC, FOLK, FUNK, HIPHOP, JAZZ, LATIN, NONMUSIC, POP, REGGAE, ROCK, STAGE };

    }
}
