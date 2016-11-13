// Set -- Parse tree node strategy for printing the special form set!

using System;

namespace Tree {
    public class Set : Special {
		public Set() { }

		public override Node eval(Node exp, Environment env) {
			int num = Begin.expLength(exp);
			if (num != 3) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Node cdar = exp.getCdr().getCar();
			Node cddar = exp.getCdr().getCdr().getCar();
			env.assign(cdar, cddar.eval(env));
			return StringLit.getUnspecific();
		}

        public override void print(Node t, int n, bool p) {
            Printer.printSet(t, n, p);
        }
    }
}