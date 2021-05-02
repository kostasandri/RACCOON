$NameSpaces$

namespace $GrammarName$
{
    class ASTGeneration : $GrammarName$BaseVisitor<int>
    {
    private CASTElement m_root;
    private Stack<CASTComposite> m_parentsStack = new Stack<CASTComposite>();
    private Stack<ContextType> m_currentContext = new Stack<ContextType>();

    public CASTElement MRoot
    {
        get { return m_root; }
        set { m_root = value; }
    }

    public ASTGeneration()
    {
        m_currentContext.Push(ContextType.CT_NA);

    }

    private ITerminalNode GetTerminalNode(ParserRuleContext node, IToken terminal)
    {

        for (int i = 0; i < node.ChildCount; i++)
        {
            ITerminalNode child = node.GetChild(i) as ITerminalNode;
            if (child != null)
            {
                if (child.Symbol == terminal)
                {
                    return child;
                }
            }
        }
        return null;
    }

    private int VisitTerminalInContext(ParserRuleContext tokenParent, IToken token, ContextType contextType)
    {
        int res;
        m_currentContext.Push(contextType);
        res = Visit(GetTerminalNode(tokenParent, token));
        m_currentContext.Pop();
        return res;
    }

    private int VisitElementInContext(IParseTree element, ContextType contextType)
    {
        int res;
        m_currentContext.Push(contextType);
        res = Visit(element);
        m_currentContext.Pop();
        return res;
    }

    private int VisitElementsInContext(IEnumerable<IParseTree> context, ContextType contextType)
    {
        int res = 0;
        m_currentContext.Push(contextType);
        foreach (ParserRuleContext elem in context)
        {
            res = Visit(elem);
        }
        m_currentContext.Pop();
        return res;
    }
$ASTGenerationVisitors$
}
