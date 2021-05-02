/*
MIT License

Copyright(c) [2016] [Grigoris Dimitroulakos]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace GrammarAnalyzer.ASTVisitor
{

    public interface IASTAbstractConcreteVisitor<Return>
    {

        Return VisitCompileUnit(CASTElement currentNode);

        Return VisitGram_Spec(CASTElement currentNode);

        Return VisitGrammar_Rule(CASTElement currentNode);

        Return VisitGrammar_Rule_Rhs(CASTElement currentNode);

        Return VisitClosure(CASTElement currentNode);

        Return VisitParenthesizedTerm(CASTElement currentNode);

        Return VisitTERMINAL(CASTElement currentNode);

        Return VisitNONTERMINAL(CASTElement currentNode);
    }

    public class CASTAbstractConcreteVisitor<Return> : CASTAbstractVisitor<Return>, IASTAbstractConcreteVisitor<Return>
    {

        public virtual Return VisitCompileUnit(CASTElement currentNode)
        {
            return base.VisitChildren(currentNode);
        }

        public virtual Return VisitGram_Spec(CASTElement currentNode)
        {
            return base.VisitChildren(currentNode);
        }

        public virtual Return VisitGrammar_Rule(CASTElement currentNode)
        {
            return base.VisitChildren(currentNode);
        }

        public virtual Return VisitGrammar_Rule_Rhs(CASTElement currentNode)
        {
            return base.VisitChildren(currentNode);
        }

        public virtual Return VisitClosure(CASTElement currentNode)
        {
            return base.VisitChildren(currentNode);
        }

        public virtual Return VisitParenthesizedTerm(CASTElement currentNode)
        {
            return base.VisitChildren(currentNode);
        }

        public virtual Return VisitTERMINAL(CASTElement currentNode)
        {
            //return base.VisitChildren(currentNode);
            return base.VisitTerminal(currentNode);
        }

        public virtual Return VisitNONTERMINAL(CASTElement currentNode)
        {
            return base.VisitTerminal(currentNode);
            //return base.VisitChildren(currentNode);
        }
    }
}
