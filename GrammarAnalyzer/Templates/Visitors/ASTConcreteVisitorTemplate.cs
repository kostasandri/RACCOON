$NameSpaces$

namespace $GrammarName$
{
    public interface IASTAbstractConcreteVisitor<Return>
    {
        $interface$
    }

    public class CASTAbstractConcreteVisitor<Return> : CASTAbstractVisitor<Return>, IASTAbstractConcreteVisitor<Return>
    {
        $implementinterface$
    }
}
