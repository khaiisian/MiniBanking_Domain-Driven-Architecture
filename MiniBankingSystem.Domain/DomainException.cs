using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBankingSystem.Domain;

public class DomainException: Exception
{
    public DomainException(string message): base(message)
    {

    }
}

// Notes
//: base(message) => Calls the parent(Exception) constructor with message.
//DomainException("bla bla") => Exception("bla bla")
