using System.Collections.Generic;
using System.Linq;
using GraphQL.Language.AST;

namespace GraphQL.Validation
{
    public class ComplexityVisitor : INodeVisitor
    {
        private readonly ValidationContext _context;
        private int counter;

        public ComplexityVisitor(ValidationContext context)
        {
            _context = context;
        }

        public void Enter(INode node)
        {
            if (counter > 0)
            {
                counter--;
                return;
            }

            var field = node as Field;
            if (field != null)
            {
                var subSelections = field.SelectionSet.Selections.Count();
                if (subSelections > 0)
                {
                    counter += subSelections;
                    if (_context.ComplexityMap.ContainsKey(node))
                        _context.ComplexityMap[node] += subSelections;
                    else
                    {
                        _context.ComplexityMap.Add(node, subSelections);
                    }
                }
                return;
            }

            var argument = node as Argument;
            if (argument != null)
            {
                if (argument.Name == "count")
                {
                    var v = argument.Value;
                }
            }
        }

        public void Leave(INode node) { }
    }
}