using System;
using System.Collections.Generic;

namespace Scribe.Exceptions
{
    public class InvalidWorkFolderException : Exception
    {
        public IEnumerable<string> Messages { get; private set; }
        public InvalidWorkFolderException(IEnumerable<string> messages, Exception ex)
        : base("The given folder is an invalid working folder for PDF generation. Examine Messages for more details.", ex)
        {
            this.Messages = messages;
        }

        public InvalidWorkFolderException(IEnumerable<string> messages)
        : base("The given folder is an invalid working folder for PDF generation. Examine Messages for more details.")
        {
            this.Messages = messages;
        }

        public InvalidWorkFolderException(string message)
        : base(message)
        {
            this.Messages = new List<string> { message };
        }
    }
}