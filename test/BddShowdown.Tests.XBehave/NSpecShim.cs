using System;
using Xbehave;

namespace BddShowdown.Tests
{
    public abstract class NSpecShim
    {
        protected void Given(string context, Action body) => ("Given " + context).x(body);
        protected void And(string context, Action body) => ("And " + context).x(body);
        protected void When(string context, Action body) => ("When " + context).x(body);
        protected void Then(string context, Action body) => ("Then " + context).x(body);
    }
}