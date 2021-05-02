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
