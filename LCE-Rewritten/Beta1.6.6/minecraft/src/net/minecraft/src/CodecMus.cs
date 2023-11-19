using System.IO;

//import paulscode.sound.codecs.CodecJOrbis;

using System;
using System.IO;

namespace YourNamespace
{
    public class CodecMus 
    {
        protected Stream OpenInputStream()
        {
            return new MusInputStream(this, Url, UrlConnection.GetInputStream());
        }
    }
}

