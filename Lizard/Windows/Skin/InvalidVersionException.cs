using System;
using System.Collections.Generic;
using System.Text;

namespace Lizard.Windows.Skin
{
    public class InvalidVersionException : Exception
    {
        private string _actualVersion;

        public string ActualVersion
        {
            get { return _actualVersion; }
        }

        private string _expectedVersion;
        public string ExpectedVersion
        {
            get { return _expectedVersion; }
        }


        public InvalidVersionException(string actualVersion, string expectedVersion)
            : base ( String.Format("Invalid schema version for form skin file. File version is {0} and expecting version {1}.",
                actualVersion, expectedVersion))
        {
            _actualVersion = actualVersion;
            _expectedVersion = expectedVersion;
        }
    }
}
