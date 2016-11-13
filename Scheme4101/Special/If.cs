// If -- Parse tree node strategy for printing the special form if

using System;

namespace Tree {
    public class If : Special {
		public If() { }

		public override Node eval(Node exp, Environment env) {
			int num = Begin.expLength(exp);
			if (num < 3 || num > 4) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Node cdar = exp.getCdr().getCar();
			Node cddar = exp.getCdr().getCdr().getCar();
			Node node;
			if (num == 3) {
				node = StringLit.getUnspecific();
			}
			else {
				node = exp.getCdr().getCdr().getCdr().getCar();
			}
			if (cdar.eval(env) != BoolLit.getInstance(false)) {
				return cddar.eval(env);
			}
			return node.eval(env);
		}

        public override void print(Node t, int n, bool p) {
            Printer.printIf(t, n, p);
        }
    }
}