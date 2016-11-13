// Cond -- Parse tree node strategy for printing the special form cond

using System;

namespace Tree {
    public class Cond : Special {
		public Cond() { }

		public override Node eval(Node exp, Environment env) {
			int num = Begin.expLength(exp);
			if (num < 2) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			return evalCases(exp.getCdr (), env);
		}

		private Node evalCases(Node exp, Environment env) {
			if (exp.isNull()) {
				return StringLit.getUnspecific();
			}
			Node car = exp.getCar();
			if (Begin.expLength(car) < 1) {
				Console.Error.WriteLine("error: invalid expression");
				return Nil.getInstance();
			}
			Node caar = car.getCar();
			Node cdar = car.getCdr();
			if (caar.isSymbol() && caar.getName().Equals("else")) {
				if (cdar.isNull() || !exp.getCdr().isNull()) {
					Console.Error.WriteLine("error: invalid expression");
					return Nil.getInstance();
				}
				return Begin.begin(cdar, env);
			}
			else {
				Node node = caar.eval(env);
				if (node == BoolLit.getInstance(false)) {
					return evalCases(exp.getCdr(), env);
				}
				if (cdar.isNull()) {
					return node;
				}
				Node cadar = cdar.getCar();
				if (!cadar.isSymbol() || !cadar.getName().Equals("=>")) {
					return Begin.begin(cdar, env);
				}
				if (Begin.expLength(cdar) != 2) {
					Console.Error.WriteLine("error: invalid expression");
					return Nil.getInstance();
				}
				Node node2 = cdar.getCdr().getCar().eval(env);
				return node2.apply(node);
			}
		}

        public override void print(Node t, int n, bool p) { 
            Printer.printCond(t, n, p);
        }
    }
}