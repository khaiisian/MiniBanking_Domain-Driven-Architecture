using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBankingSystem.Domain.ValueObjects;

public record AccountId(Guid Value)
{
    public static AccountId New()
    {
        return new AccountId(Guid.NewGuid());
    }
}

// Nature of Primary Constructor.
//public record AccountId(Guid Value);
//The compiler automatically creates
//- the constructor
//- the Value property
//- and assigns the value for you

// Equal to this

//public record AccountId
//{
//    public Guid Value { get; }

//    public AccountId(Guid value)
//    {
//        Value = value;
//    }
//}