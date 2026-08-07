# Banking — A Domain-Driven Design (DDD) Learning Project

A small **banking bounded context** built in **C# / .NET 10**, created to learn Domain-Driven Design by doing.
The goal isn't to ship a bank — it's to *feel* the shape of a well-modeled domain: rich models, value
objects, aggregates, and layered architecture.

---

## Table of Contents
1. [What is DDD? (the core idea)](#1-what-is-ddd-the-core-idea)
2. [The 4-Layer Architecture](#2-the-4-layer-architecture)
3. [Fake DDD vs Real DDD](#3-fake-ddd-vs-real-ddd)
4. [The Building Blocks](#4-the-building-blocks)
5. [Project Scope & Flow](#5-project-scope--flow)
6. [Project Structure](#6-project-structure)
7. [What We've Built So Far](#7-what-weve-built-so-far)
8. [Key Learnings](#8-key-learnings)
9. [Roadmap](#9-roadmap)

---

## 1. What is DDD? (the core idea)

> **Model the business faithfully in code — using the business's own words, with the rules living inside the model.**

DDD is **not** a tech stack and **not** a folder structure. It's a way of *thinking and modeling*.

- **"Domain"** = the business you're building software for (here: banking).
- **"Domain-Driven"** = let the business drive how you design the code.
- **Ubiquitous Language** = code speaks the same words as the business experts
  (`account.Withdraw(amount)`, not `UpdateBalance(3)`).

---

## 2. The 4-Layer Architecture

```
   Presentation ──▶ Application ──▶  DOMAIN  ◀── Infrastructure
     (waiter)       (coordinator)    (chef *)      (fridge/oven/DB)
                                        ▲
                          everything depends INWARD;
                          the Domain depends on nothing
```

| Layer | Analogy | Responsibility |
|---|---|---|
| **Presentation** | The waiter | UI / API / console. Takes requests, shows results. No rules. |
| **Application** | The coordinator | Thin. Orchestrates steps: get → act → save. No rules. |
| **Domain** * | The chef & recipes | The heart. All business concepts and rules live here. |
| **Infrastructure** | Fridge / oven / suppliers | Database, email, payment providers — replaceable plumbing. |

**The golden rule:** dependencies point **inward** toward the Domain. The Domain depends on nothing,
so the valuable business rules stay clean and independent of databases and screens.

**The repository trick (dependency inversion):** the Domain defines the repository *interface*
(`IAccountRepository`), and Infrastructure provides the *implementation*. That's how the Domain stays
database-agnostic.

---

## 3. Fake DDD vs Real DDD

Same folders, same tech — the difference is **where the rules live and whether the model protects itself.**

```csharp
// FAKE DDD (anemic): a dumb data bag; the rule floats in a service and can be bypassed
class Account { public decimal Balance { get; set; } }
account.Balance -= 500;   // nothing stops an overdraft

// REAL DDD (rich): data + behavior together; the rule is enforced from inside
class Account {
    public decimal Balance { get; private set; }
    public void Withdraw(decimal amount) {
        if (amount > Balance) throw new DomainException("Insufficient funds"); // rule lives HERE
        Balance -= amount;
    }
}
account.Balance -= 500;   // won't even compile
account.Withdraw(500);    // the only guarded door in
```

- **Anemic model** (data with no behavior, rules scattered in services) = *fake* DDD.
- **Rich model** (data + rules together, self-protecting) = *real* DDD. **This is the core.**

---

## 4. The Building Blocks

Pick the right one by asking: **"How do I know if two of these are the same?"**

| Ask... | It's a... | Example |
|---|---|---|
| Same if the **values** match? (immutable) | **Value Object** | `Money`, `Email`, `DateRange` |
| Same if the **ID** matches? (changes over time) | **Entity** | `Account`, `Order`, `Customer` |
| A group that must stay **consistent together**, with one front door? | **Aggregate** | `Order` + its `OrderLine`s |

They nest: **Value Objects live inside Entities; Entities group into Aggregates.**
The **Aggregate Root** is the only "front door" — outside code never reaches inside.

### How do I decide: Entity or Value Object?

Ask one question about the thing:

> **"If two of them have the exact same details, are they the same thing — or still two different things?"**

| Thing | Two with identical details... | So it's a... |
|---|---|---|
| A $10 note | ...are interchangeable — nobody cares which | **Value Object** |
| A bank account | ...are still *different* accounts (two people can both have $0) | **Entity** |
| The color "red" | ...are the same | **Value Object** |
| A person | ...are still two different people | **Entity** |

**Shortcut:**
- Has a unique **ID** and is **tracked over time** (its details change but it stays "the same one")? → **Entity** (`Account`)
- You only care **what it is**, not **which one**, and it never changes? → **Value Object** (`Money`, `AccountId`)

One more test if stuck: *"Can I change it and have it still be the same thing?"* Yes → **Entity**.
No — "changing" just means using a different one → **Value Object**.

---

## 5. Project Scope & Flow

### Use cases
| # | Use case | Business rule it protects |
|---|---|---|
| 1 | Open an account | Starts at zero balance |
| 2 | Deposit money | Amount must be positive |
| 3 | Withdraw money | Positive **and** can't exceed balance (no overdraft) |

**Out of scope (on purpose):** real database, login, interest, transfers, web UI.
We use an in-memory store and a console app — we're learning the *shape*.

### Flow of a "Withdraw $50" request
```
Presentation  →  builds a WithdrawCommand
Application   →  1. repo.GetById(id)      (Infrastructure)
                 2. account.Withdraw($50) (Domain enforces the rule)
                 3. repo.Save(account)     (Infrastructure)
Domain        →  if $50 > balance -> reject; else balance -= $50; raise MoneyWithdrawn
Infrastructure→  actually stores/loads the Account
```
Three things to notice: the **rule lives in the Domain**, the **Application is dumb glue**, and all
**dependencies point inward**.

---

## 6. Project Structure

```
DDD Architecture/
├── Banking.sln
└── src/
    ├── Banking.Domain/          <- references NOTHING  (the rule, made physical)
    │   └── ValueObjects/
    │       └── Money.cs
    ├── Banking.Application/       <- references Domain
    ├── Banking.Infrastructure/    <- references Domain
    └── Banking.ConsoleApp/        <- references Application + Infrastructure  (startup)
```

---

## 7. What We've Built So Far

### Step 0 — The layered skeleton
Four projects with references pointing inward. `Banking.Domain` references nothing — the dependency
rule enforced by the compiler.

### Step 1 — The `Money` Value Object
```csharp
namespace Banking.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Of(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.");
        return new Money(amount, currency);
    }

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException("Cannot add different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException("Cannot subtract different currencies.");
        return Of(Amount - other.Amount, Currency);
    }
}
```

Design choices that make it a proper Value Object:
- **`record`** → value equality (two `Money` with equal amount+currency are equal).
- **`get;` with no `set;`** → immutable (safe to share, validated-once-valid-forever).
- **private constructor + `Of(...)` factory** → invalid money is impossible to construct
  (*make illegal states unrepresentable*).
- **behavior inside the type** (`Add`, `Subtract`) → a rich model, not a data bag.

---

## 8. Key Learnings

### `record` vs `class` — value vs reference equality
- **`class`** compares by **reference**: "is it the exact same object?"
  `new Money(100,"USD") == new Money(100,"USD")` → **False**.
- **`record`** compares by **value**: "do they hold the same values?"
  `Money.Of(100,"USD") == Money.Of(100,"USD")` → **True**.
- A Value Object is defined by its values, so `record` is the right choice.

### Why `Money` is immutable (`get;` only)
Making it changeable would break three things:
1. **Shared-object corruption** — if two accounts reused the same `Money` and one mutated it, the
   other would change too ("spooky action at a distance").
2. **Bypassed validation** — a setter lets code skip `Of(...)` and set a negative amount.
3. **Broken equality** — value equality needs stable contents (e.g. for dictionary keys).

The deeper reason: **values don't change.** The number `10` never *becomes* `11`; you use a different
number. `$100` never *becomes* `$150`; you make a new `Money`. **Entities change; values are replaced.**

---
