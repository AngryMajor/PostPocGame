using System;
using System.Collections.Generic;

namespace PostPocModel
{
    public static class Helper {
        public static void CheckForMissingArgument(string fieldName, Dictionary<string, object> args) {
           if (args.ContainsKey(fieldName) == false)
                throw new MissingFieldException("arguments are missing Field: " + fieldName);
        }
    }
}
